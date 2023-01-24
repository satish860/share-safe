using Amazon.S3;
using Amazon.S3.Transfer;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ShareSafe.API.Files.UploadFile
{
    public class UploadFileEndpoint : EndpointWithoutRequest
    {
        private readonly IAmazonS3 amazonS3Client;
        private readonly IConfiguration configuration;
        private readonly IMongoCollection<FileMetadata> collection;

        public UploadFileEndpoint(IAmazonS3 amazonS3Client,
                                  IConfiguration configuration,
                                  IMongoCollection<FileMetadata> collection)
        {
            this.amazonS3Client = amazonS3Client;
            this.configuration = configuration;
            this.collection = collection;
        }

        public override void Configure()
        {
            Post("/files/{fileId}/upload");
            AllowFileUploads();
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
                TransferUtility transferUtility = new TransferUtility(amazonS3Client);
                var bucketName = configuration["DOCTL:BucketName"];

                if (Files.Count > 0)
                {
                    await transferUtility.UploadAsync(stream: this.Files[0].OpenReadStream(),
                                                      bucketName,
                                                      filemetadata.Name,
                                                      ct);
                }
                await SendNoContentAsync();
            }
        }
    }
}
