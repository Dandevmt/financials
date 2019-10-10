﻿using Financials.Application.Repositories;
using Financials.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Database
{
    public class CredentialRepository : ICredentialRepository
    {
        private readonly IMongoCollection<Credentials> collection;
        private readonly IClientSessionHandle session;

        public CredentialRepository(IMongoDatabase mongo, IClientSessionHandle session)
        {
            this.collection = mongo.GetCollection<Credentials>(nameof(Credentials));
            this.session = session;
        }

        public Credentials Add(Credentials credentials)
        {
            collection.InsertOne(session, credentials);
            return credentials;
        }

        public Credentials Get(Guid userId)
        {
            return collection.Find(session, FilterByUserId(userId)).FirstOrDefault();
        }

        public Credentials Get(string email)
        {
            return collection.Find(session, FilterByEmail(email)).FirstOrDefault();
        }

        public Credentials UpdateOne(Credentials credentials)
        {
            var result = collection.ReplaceOne(FilterByUserId(credentials.UserId), credentials);
            if (result.IsAcknowledged)
            {
                return credentials;
            } else
            {
                return null;
            }
        }

        private FilterDefinition<Credentials> FilterByUserId(Guid userId)
        {
            return Builders<Credentials>.Filter.Eq(c => c.UserId, userId);
        }

        private FilterDefinition<Credentials> FilterByEmail(string email)
        {
            return Builders<Credentials>.Filter.Eq(c => c.Email, email);
        }
    }
}
