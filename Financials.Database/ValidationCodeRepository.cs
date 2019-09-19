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
        private readonly IClientSessionHandle session;
        public ValidationCodeRepository(IMongoDatabase mongo)
        {
            collection = mongo.GetCollection<ValidationCode>(nameof(ValidationCode));
        }
        public ValidationCodeRepository(IMongoDatabase mongo, IClientSessionHandle session)
            : this(mongo)
        {
            this.session = session;
        }

        public ValidationCode Add(ValidationCode code)
        {
            return collection.FindOneAndReplace(session, FilterUserIdAndType(code.UserId, code.Type), code, new FindOneAndReplaceOptions<ValidationCode>()
            {
                IsUpsert = true
            });
        }

        public bool Delete(Guid userId, ValidationCodeType type)
        {
            var result = collection.DeleteOne(session, FilterUserIdAndType(userId, type));
            return result.DeletedCount >= 1;
        }

        public ValidationCode Get(Guid userId, ValidationCodeType type)
        {
            return collection.Find(session, FilterUserIdAndType(userId, type)).FirstOrDefault();
        }

        private FilterDefinition<ValidationCode> FilterUserIdAndType(Guid userId, ValidationCodeType type)
        {
            return Builders<ValidationCode>.Filter.Eq(v => v.UserId, userId) & Builders<ValidationCode>.Filter.Eq(v => v.Type, type);
        }
    }
}
