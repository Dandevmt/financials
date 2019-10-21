using System;
using System.Collections.Generic;
using System.Text;
using Financials.Entities;

namespace Financials.Application.Transactions.Repositories
{
    public interface ITransactionRepository
    {
        Entities.Transaction Add(Transaction transaction);
    }
}
