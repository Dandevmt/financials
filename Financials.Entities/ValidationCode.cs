using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Entities
{
    public class ValidationCode
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Guid UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Code { get; set; }
        public ValidationCodeType Type { get; set; }
    }
}
