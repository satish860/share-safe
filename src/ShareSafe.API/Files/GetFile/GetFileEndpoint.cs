using MongoDB.Bson;
using MongoDB.Driver;

namespace ShareSafe.API.Files.GetFile
{
    public class GetFileEndpoint : EndpointWithoutRequest
    {
        private readonly IMongoCollection<FileMetadata> collection;

        public GetFileEndpoint(IMongoCollection<FileMetadata> collection)
        {
            this.collection = collection;
        }

        public override void Configure()
        {
            Get("/files/{fileid}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
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
