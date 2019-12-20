using Financials.Application.UserManagement.Repositories;
using Financials.UserManagement;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Financials.Database
{
    public class UserRepository : UserManagement.IUserRepository
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
            return collection.FindOneAndReplace(session, x => x.Id == user.Id, user);
        }

        public User Delete(ObjectId id)
        {
            var update = Builders<User>.Update.Set(u => u.Archived, DateTime.Now);
            return collection.FindOneAndUpdate(session, x => x.Id == id, update);
        }

        public UserManagement.User GetByUsername(string username)
        {
            return collection.Find(session, x => x.Username == username).FirstOrDefault();
        }
    }
}
