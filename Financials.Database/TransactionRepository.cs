using Financials.Application.Transactions.Repositories;
using Financials.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Database
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IMongoCollection<Transaction> collection;
        private readonly IClientSessionHandle session;

        public TransactionRepository(IMongoDatabase mongo, IClientSessionHandle session)
        {
            this.collection = mongo.GetCollection<Transaction>(nameof(Transaction));
            this.session = session;
        }

        public Transaction Add(Transaction transaction)
        {
            collection.InsertOne(transaction);
            return transaction;
        }
    }
}
