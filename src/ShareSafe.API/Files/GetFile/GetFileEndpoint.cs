using MongoDB.Bson;
using MongoDB.Driver;

namespace ShareSafe.API.Files.GetFile
{
    public class GetFileEndpoint : EndpointWithoutRequest
    {
        private readonly IMongoClient mongoClient;
        private readonly IConfiguration configuration;

        public GetFileEndpoint(IMongoClient mongoClient, IConfiguration configuration)
        {
            this.mongoClient = mongoClient;
            this.configuration = configuration;
        }

        public override void Configure()
        {
            Get("/files/{fileid}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var DBName = this.configuration["DBNAME"];
            var database = mongoClient.GetDatabase(DBName);
            var collection = database.GetCollection<FileMetadata>("files");
            var fileId = Route<ObjectId>("fileid");
            var fileMetadata = await collection.Find(p => p.Id == fileId)
                                               .FirstOrDefaultAsync();
            if (fileMetadata == null)
                await SendNotFoundAsync();
            await SendOkAsync(response: fileMetadata ?? new FileMetadata(),
                              cancellation: ct);

        }
    }
}
