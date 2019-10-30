using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Entities
{
    public class Tenant
    {
        [BsonId, BsonRepresentation(BsonType.String)]
        public ObjectId TenantId { get; set; }
        public string TenantName { get; set; }
    }
}
