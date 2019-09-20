using Financials.Application.Repositories;
using Financials.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Database
{
    public class ValidationCodeRepository : IValidationCodeRepository
    {
        private readonly IMongoCollection<ValidationCode> collection;
        public ValidationCodeRepository(IMongoDatabase mongo)
        {
            collection = mongo.GetCollection<ValidationCode>(nameof(ValidationCode));
        }

        public ValidationCode Add(ValidationCode code)
        {
            return collection.FindOneAndReplace(FilterUserIdAndType(code.UserId, code.Type), code, new FindOneAndReplaceOptions<ValidationCode>()
            {
                IsUpsert = true
            });
        }

        public bool Delete(Guid userId, ValidationCodeType type)
        {
            var result = collection.DeleteOne(FilterUserIdAndType(userId, type));
            return result.DeletedCount >= 1;
        }

        public ValidationCode Get(Guid userId, ValidationCodeType type)
        {
            return collection.Find(FilterUserIdAndType(userId, type)).FirstOrDefault();
        }

        private FilterDefinition<ValidationCode> FilterUserIdAndType(Guid userId, ValidationCodeType type)
        {
            return Builders<ValidationCode>.Filter.Eq(v => v.UserId, userId) & Builders<ValidationCode>.Filter.Eq(v => v.Type, type);
        }
    }
}
