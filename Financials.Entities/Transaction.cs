using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Entities
{
    public class Transaction
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Guid UserId { get; set; }
        public TransactionType Type { get; set; }
        public double Amount { get; set; }
        public int CheckNumber { get; set; }
        public string Description { get; set; }
        public bool GoodsOrServicesGiven { get; set; }
        public DateTime Date { get; set; }
    }
}
