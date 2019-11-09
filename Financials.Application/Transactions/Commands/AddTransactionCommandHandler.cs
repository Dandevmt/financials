using Financials.Application.Transactions.Repositories;
using Financials.Application.UserManagement.Repositories;
using Financials.Application.UserManagement.Security;
using Financials.CQRS;
using Financials.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.Transactions.Commands
{
    public class AddTransactionCommandHandler : ICommandHandler<AddTransactionCommand>
    {
        private readonly ITransactionRepository trRepo;
        private readonly IUserRepository userRepo;
        public AddTransactionCommandHandler(ITransactionRepository trRepo, IUserRepository userRepo)
        {
            this.trRepo = trRepo;
            this.userRepo = userRepo;
        }
        public Task<CommandResult> Handle(AddTransactionCommand command)
        {
            if (!command.Validate(out ValidationError error))
                return CommandResult.Fail(error).AsTask();

            var user = userRepo.Get(command.UserId);
            if (user == null)
                return CommandResult.Fail(UserManagement.UserManagementError.UserNotFound()).AsTask();

            var transaction = trRepo.Add(new Transaction()
            {
                Amount = command.Amount,
                CheckNumber = command.CheckNumber,
                Date = command.Date,
                Description = command.Description,
                GoodsOrServicesGiven = command.GoodsOrServicesGiven,
                Type = (Entities.TransactionType)command.Type,
                UserId = command.UserId
            });

            return CommandResult<string>.Success(transaction.Id.ToString()).AsTask();
        }
    }
}
