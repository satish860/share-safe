using MongoDB.Bson;
using MongoDB.Driver;

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
            var collection = database.GetCollection<CreateFile>("files");
            req.Id = ObjectId.GenerateNewId();
            await collection.InsertOneAsync(req);
            // Get the File and Create the ID and store it in MongoDB
            await SendOkAsync(req.Id);
        }
    }
}
