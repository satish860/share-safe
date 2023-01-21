using MongoDB.Bson;

namespace ShareSafe.API.Files
{
    public class FileMetadata
    {
        public ObjectId Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? OwnedBy { get; set; }

        public string? Signature { get; set; }
    }
}
