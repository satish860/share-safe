using Amazon.S3;
using Amazon.S3.Transfer;

namespace ShareSafe.API.Files.UploadFile
{
    public class UploadFileEndpoint : EndpointWithoutRequest
    {
        private readonly IAmazonS3 amazonS3Client;
        private readonly IConfiguration configuration;

        public UploadFileEndpoint(IAmazonS3 amazonS3Client, IConfiguration configuration)
        {
            this.amazonS3Client = amazonS3Client;
            this.configuration = configuration;
        }

        public override void Configure()
        {
            Post("/files/{fileId}/upload");
            AllowFileUploads();
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            TransferUtility transferUtility = new TransferUtility(amazonS3Client);
            var bucketName = configuration["DOCTL:BucketName"];

            if (Files.Count > 0)
            {
                await transferUtility.UploadAsync(stream: this.Files[0].OpenReadStream(),
                                                  bucketName,
                                                  this.Files[0].Name,
                                                  ct);
            }
            await SendNoContentAsync();
        }
    }
}
