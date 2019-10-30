using Financials.Application.UserManagement.Repositories;
using Financials.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Database
{
    public class TenantRepository : ITenantRepository
    {
        private readonly IMongoCollection<Tenant> collection;
        private readonly IClientSessionHandle session;

        public TenantRepository(IMongoDatabase mongo, IClientSessionHandle session)
        {
            this.collection = mongo.GetCollection<Tenant>(nameof(Tenant));
            this.session = session;
        }

        public Tenant Add(Tenant tenant)
        {
            collection.InsertOne(session, tenant);
            return tenant;
        }

        public Tenant Get(string id)
        {
            return collection.Find(session, t => t.TenantId == id).FirstOrDefault();
        }

        public IList<Tenant> GetAll()
        {
            return collection.Find(session, t => true).ToList();
        }

        public Tenant Update(Tenant tenant)
        {
            var fd = Builders<Tenant>.Filter.Eq(t => t.TenantId, tenant.TenantId);
            var result = collection.ReplaceOne(session, fd, tenant);
            if (result.IsAcknowledged)
            {
                return tenant;
            }
            else
            {
                return null;
            }
        }
    }
}
