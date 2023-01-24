using Amazon.S3;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ShareSafe.API.Files.DownloadFile
{
    public class DownloadFileEndpoint : EndpointWithoutRequest
    {
        private readonly IMongoCollection<FileMetadata> collection;
        private readonly IAmazonS3 amazonS3;
        private readonly IConfiguration configuration;

        public DownloadFileEndpoint(IMongoCollection<FileMetadata> collection,
                                    IAmazonS3 amazonS3,
                                    IConfiguration configuration)
        {
            this.collection = collection;
            this.amazonS3 = amazonS3;
            this.configuration = configuration;
        }

        public override void Configure()
        {
            Get("/files/{fileId}/download");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var fileId = Route<ObjectId>("fileId");
            var filemetadata = await this.collection
                                         .Find(p => p.Id == fileId)
                                         .FirstOrDefaultAsync();
            if (filemetadata == null)
            {
                await SendNotFoundAsync();
            }
            else
            {
                var bucketName = configuration["DOCTL:BucketName"];
                var response = await amazonS3.GetObjectAsync(bucketName, filemetadata.Name);
                await SendStreamAsync(response.ResponseStream,
                        contentType: response.Headers.ContentType,
                        enableRangeProcessing: true);
            }
        }
    }
}
