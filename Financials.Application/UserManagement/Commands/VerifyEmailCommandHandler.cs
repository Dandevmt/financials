using Financials.Application.CQRS;
using Financials.Application.UserManagement.Repositories;
using Financials.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.UserManagement.Commands
{
    public class VerifyEmailCommandHandler : ICommandHandler<VerifyEmailCommand>
    {
        private IValidationCodeRepository codeRepo;
        private ICredentialRepository credRepo;

        public VerifyEmailCommandHandler(IValidationCodeRepository codeRepo, ICredentialRepository credRepo)
        {
            this.codeRepo = codeRepo;
            this.credRepo = credRepo;
        }

        public Task<CommandResult> Handle(VerifyEmailCommand input)
        {
            var code = codeRepo.Get(input.UserId, ValidationCodeType.Email);
            if (code == null || !code.Code.Equals(input.Code) || (DateTime.Today - code.CreatedDate).TotalMinutes > 15)
                return CommandResult.Fail(UserManagementError.InvalidEmailVerificationCode()).AsTask();

            var creds = credRepo.Get(input.UserId);
            creds.EmailVerified = DateTime.Now;

            var res = credRepo.UpdateOne(creds);
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
