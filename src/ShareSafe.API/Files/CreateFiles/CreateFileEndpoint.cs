using MongoDB.Bson;
using MongoDB.Driver;
using ShareSafe.API.Files.GetFile;

namespace ShareSafe.API.Files.CreateFiles
{
    public class CreateFileEndpoint : Endpoint<CreateFile>
    {
        
        private readonly IMongoCollection<FileMetadata> collection;

        public CreateFileEndpoint(IMongoCollection<FileMetadata> collection)
        {
            this.collection = collection;
        }

        public override void Configure()
        {
            Post("/files");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CreateFile req, CancellationToken ct)
        { 
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
