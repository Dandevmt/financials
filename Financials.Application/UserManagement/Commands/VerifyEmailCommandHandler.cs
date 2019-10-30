using Financials.Application.CQRS;
using Financials.Application.UserManagement.Repositories;
using Financials.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.UserManagement.Commands
{
    public class VerifyEmailCommandHandler : ICommandHandler<VerifyEmailCommand>
    {
        private IUserRepository userRepo;

        public VerifyEmailCommandHandler(IUserRepository userRepo)
        {
            this.userRepo = userRepo;
        }

        public Task<CommandResult> Handle(VerifyEmailCommand input)
        {
            var user = userRepo.Get(input.UserId);
            if (user == null)
                return CommandResult.Fail(UserManagementError.UserNotFound("User not found")).AsTask();

            var code = user.ValidationCodes.FirstOrDefault(c => c.Type == ValidationCodeType.Email);
            if (code == null || !code.Code.Equals(input.Code) || (DateTime.Today - code.CreatedDate).TotalMinutes > 15)
                return CommandResult.Fail(UserManagementError.InvalidEmailVerificationCode()).AsTask();

            var creds = user.Credentials;
            creds.EmailVerified = DateTime.Now;

            var res = userRepo.Update(user);
            if (res == null)
            {
                return CommandResult.Fail(UserManagementError.EmailCouldNotUpdateDatabase()).AsTask();
            } else
            {
                return CommandResult.Success().AsTask();
            }
        }
    }
}
