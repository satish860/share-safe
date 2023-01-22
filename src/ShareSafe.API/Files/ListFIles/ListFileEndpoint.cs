using FastEndpoints;
using MongoDB.Driver;

namespace ShareSafe.API.Files.ListFIles
{
    public class ListFileEndpoint : EndpointWithoutRequest<IEnumerable<ListFileResponse>>
    {
        private readonly IMongoCollection<FileMetadata> collection;

        public ListFileEndpoint(IMongoCollection<FileMetadata> collection)
        {
            this.collection = collection;
        }

        public override void Configure()
        {
            Get("/files");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            List<FileMetadata> fileMetadata = await collection
                                                   .Find(Builders<FileMetadata>.Filter.Empty)
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
