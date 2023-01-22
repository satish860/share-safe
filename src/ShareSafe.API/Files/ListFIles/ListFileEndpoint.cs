using FastEndpoints;
using MongoDB.Driver;

namespace ShareSafe.API.Files.ListFIles
{
    public class ListFileEndpoint : EndpointWithoutRequest<IEnumerable<ListFileResponse>>
    {
        private readonly IMongoClient mongoClient;
        private readonly IConfiguration configuration;

        public ListFileEndpoint(IMongoClient mongoClient, IConfiguration configuration)
        {
            this.mongoClient = mongoClient;
            this.configuration = configuration;
        }

        public override void Configure()
        {
            Get("/files");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var DBName = this.configuration["DBNAME"];
            var database = mongoClient.GetDatabase(DBName);
            var collection = database.GetCollection<FileMetadata>("files");
            List<FileMetadata> fileMetadata = await collection
                                                   .Find<FileMetadata>(Builders<FileMetadata>.Filter.Empty)
                                                   .ToListAsync();

            if (fileMetadata.Count == 0)
            {
                await SendNotFoundAsync(ct);
            }
            else
            {
                var response = fileMetadata.Select(p =>
                 {
                     return new ListFileResponse
                     {
                         Description = p.Description,
                         Name = p.Name,
                         OwnedBy = p.OwnedBy,
                         Signature = p.Signature,
                     };
                 });
                await SendOkAsync(response);
            }

        }
    }
}
