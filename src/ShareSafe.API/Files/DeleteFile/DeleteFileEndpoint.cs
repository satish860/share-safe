using FastEndpoints;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ShareSafe.API.Files.DeleteFile
{
    public class DeleteFileEndpoint : EndpointWithoutRequest
    {
        private readonly IMongoCollection<FileMetadata> collection;

        public DeleteFileEndpoint(IMongoCollection<FileMetadata> collection)
        {
            this.collection = collection;
        }

        public override void Configure()
        {
            Delete("/files/{fileid}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var fileId = Route<ObjectId>("fileid");
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
