using Financials.Application.UserManagement.Repositories;
using Financials.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Financials.Database
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> collection;
        private readonly IClientSessionHandle session;
        public UserRepository(IMongoDatabase mongo, IClientSessionHandle session)
        {
            this.collection = mongo.GetCollection<User>(nameof(User));
            this.session = session;
        }

        public User Add(User user)
        {            
            collection.InsertOne(session, user);
            return user;
        }

        public User Update(User user)
        {
            return collection.FindOneAndReplace(session, FilterId(user.Id), user);
        }

        public User Delete(Guid id)
        {
            var update = Builders<User>.Update.Set(u => u.Archived, DateTime.Now);
            return collection.FindOneAndUpdate(session, FilterId(id), update);
        }

        public User Get(Guid id)
        {
            return collection.Find(session, FilterId(id)).FirstOrDefault();
        }

        public IEnumerable<User> Get(int pageSize, int pageNumber, string sortField)
        {
            return collection.Find(session, null)
                .Sort(Builders<User>.Sort.Ascending(sortField))
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize).ToEnumerable();
        }

        public IEnumerable<User> GetAll()
        {
            return collection.Find(session, f => true).ToEnumerable();
        }

        private FilterDefinition<User> FilterId(Guid id)
        {
            return Builders<User>.Filter.Eq(u => u.Id, id);
        }
    }
}
