using FastEndpoints;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ShareSafe.API.Files.DeleteFile
{
    public class DeleteFileEndpoint:EndpointWithoutRequest
    {
        private readonly IMongoClient mongoClient;
        private readonly IConfiguration configuration;

        public DeleteFileEndpoint(IMongoClient mongoClient,IConfiguration configuration)
        {
            this.mongoClient = mongoClient;
            this.configuration = configuration;
        }

        public override void Configure()
        {
            Delete("/files/{fileid}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var fileId = Route<ObjectId>("fileid");
            var DBName = this.configuration["DBNAME"];
            var database = mongoClient.GetDatabase(DBName);
            var collection = database.GetCollection<FileMetadata>("files");
            var deleteResult = await collection.DeleteOneAsync(p => p.Id == fileId);
            if (deleteResult.IsAcknowledged)
            {
                await SendAsync(deleteResult, 202, cancellation: ct);
            }
            else
            {
                await SendErrorsAsync();
            }
        }
    }
}
