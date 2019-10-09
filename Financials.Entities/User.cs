using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Entities
{
    public class User
    {
        [BsonId, BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public UserProfile Profile { get; set; }
        public DateTime Archived { get; set; }
        public HashSet<string> Permissions { get; set; }

    }
}
