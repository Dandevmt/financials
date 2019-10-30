using Financials.Application.UserManagement.Repositories;
using Financials.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Database
{
    //public class ValidationCodeRepository : IValidationCodeRepository
    //{
    //    //private readonly IMongoCollection<ValidationCode> collection;
    //    //public ValidationCodeRepository(IMongoDatabase mongo)
    //    //{
    //    //    collection = mongo.GetCollection<ValidationCode>(nameof(ValidationCode));
    //    //}

    //    //public ValidationCode Add(ValidationCode code)
    //    //{
    //    //    collection.InsertOne(code);
    //    //    return code;
    //    //}

    //    //public bool Delete(Guid userId, ValidationCodeType type)
    //    //{
    //    //    var result = collection.DeleteOne(FilterUserIdAndType(userId, type));
    //    //    return result.DeletedCount >= 1;
    //    //}

    //    //public ValidationCode Get(Guid userId, ValidationCodeType type)
    //    //{
    //    //    return collection.Find(FilterUserIdAndType(userId, type)).FirstOrDefault();
    //    //}

    //    //public IList<ValidationCode> GetAll(ValidationCodeType type)
    //    //{
    //    //    return collection.Find(f => f.Type == type).ToList();
    //    //}

    //    //public ValidationCode GetFederationCode(string federationCode)
    //    //{
    //    //    return collection.Find(FilterFederationCode(federationCode)).FirstOrDefault();
    //    //}

    //    //private FilterDefinition<ValidationCode> FilterUserIdAndType(Guid userId, ValidationCodeType type)
    //    //{
    //    //    return Builders<ValidationCode>.Filter.Eq(v => v.UserId, userId) & Builders<ValidationCode>.Filter.Eq(v => v.Type, type);
    //    //}

    //    //private FilterDefinition<ValidationCode> FilterFederationCode(string federationCode)
    //    //{
    //    //    return Builders<ValidationCode>.Filter.Eq(v => v.Code, federationCode) & Builders<ValidationCode>.Filter.Eq(v => v.Type, ValidationCodeType.Federation);
    //    //}
    //}
}
