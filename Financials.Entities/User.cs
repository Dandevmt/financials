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
        public ICollection<UserTenant> Tenants { get; set; }
        public Credentials Credentials { get; set; }
        public UserProfile Profile { get; set; }
        public ICollection<ValidationCode> ValidationCodes { get; set; }
        public DateTime Archived { get; set; }
        public bool Registered { get; set; }
    }
}
