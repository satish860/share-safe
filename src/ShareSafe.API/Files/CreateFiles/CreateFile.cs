﻿using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace ShareSafe.API.Files.CreateFiles
{
    public class CreateFile
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? OwnedBy { get; set; }

        public string? Signature { get; set; }
    }
}
