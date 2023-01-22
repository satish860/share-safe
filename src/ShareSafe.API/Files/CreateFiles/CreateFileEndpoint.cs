using MongoDB.Bson;
using MongoDB.Driver;
using ShareSafe.API.Files.GetFile;

namespace ShareSafe.API.Files.CreateFiles
{
    public class CreateFileEndpoint : Endpoint<CreateFile>
    {
        private readonly IMongoClient mongoClient;
        private readonly IConfiguration configuration;

        public CreateFileEndpoint(IMongoClient mongoClient, IConfiguration configuration)
        {
            this.mongoClient = mongoClient;
            this.configuration = configuration;
        }

        public override void Configure()
        {
            Post("/files");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CreateFile req, CancellationToken ct)
        {
            var DBName = this.configuration["DBNAME"];
            var database = mongoClient.GetDatabase(DBName);
            var collection = database.GetCollection<FileMetadata>("files");
            FileMetadata fileMetadata = new()
            {
                Name = req.Name,
                Description = req.Description,
                Id = ObjectId.GenerateNewId(),
                OwnedBy = req.OwnedBy,
                Signature = req.Signature,
            };
            await collection.InsertOneAsync(fileMetadata, cancellationToken: ct);

            await SendCreatedAtAsync<GetFileEndpoint>(new { fileid = fileMetadata.Id.ToString() }, fileMetadata);
        }


    }
}
