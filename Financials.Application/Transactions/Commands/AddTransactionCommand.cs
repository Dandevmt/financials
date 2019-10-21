using Financials.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.Transactions.Commands
{
    public class AddTransactionCommand : ICommand
    {
        public Guid UserId { get; set; }
        public TransactionType Type { get; set; }
        public double Amount { get; set; }
        public int CheckNumber { get; set; }
        public string Description { get; set; }
        public bool GoodsOrServicesGiven { get; set; }
        public DateTime Date { get; set; }

        public bool Validate(out ValidationError error)
        {
            ValidationError errors = ValidationError.New();

            if (UserId == null)
                errors.AddError(nameof(UserId), $"{nameof(UserId)} is required");

            if (Date == DateTime.MinValue || Date == DateTime.MaxValue)
                errors.AddError(nameof(Date), $"{nameof(Date)} must be a valid date");

            error = errors.HasError ? errors : null;
            return !errors.HasError;
        }
    }
}
