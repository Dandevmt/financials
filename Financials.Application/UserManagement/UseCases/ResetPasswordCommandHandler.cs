using Financials.Application.CQRS;
using Financials.Application.Email;
using Financials.Application.UserManagement.Repositories;
using Financials.Application.UserManagement.Security;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.UserManagement.UseCases
{
    public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand>
    {
        private IValidationCodeRepository codeRepo;
        private ICredentialRepository credRepo;
        private IPasswordHasher hasher;
        private IEmailSender emailSender;
        public ResetPasswordCommandHandler(
            IValidationCodeRepository codeRepo, 
            ICredentialRepository credRepo,
            IPasswordHasher hasher,
            IEmailSender emailSender)
        {
            this.codeRepo = codeRepo;
            this.credRepo = credRepo;
            this.hasher = hasher;
            this.emailSender = emailSender;
        }

        public async Task<CommandResult> Handle(ResetPasswordCommand input)
        {
            if (!input.Validate(out ValidationError error))
                return CommandResult.Fail(error);

            var code = codeRepo.Get(input.UserId, Entities.ValidationCodeType.PasswordReset);
            var creds = credRepo.Get(input.UserId);

            bool codesMatch = code != null && code.Code.Equals(input.ResetCode);
            bool passMatch = creds != null && hasher.VerifyPassword(creds.Password, input.NewPassword);

            if (!codesMatch && !passMatch)
            {
                return CommandResult.Fail(UserManagementError.InvalidCodePasswordOrUserId());             
            }                

            // Creds shouldn't be null
            if (creds == null)
            {
                return CommandResult.Fail(UserManagementError.InvalidCodePasswordOrUserId());
            }                

            creds.Password = hasher.HashPassword(input.NewPassword);

            credRepo.UpdateOne(creds);

            await emailSender.Send(new PasswordWasResetEmail() 
            {
                To = creds.Email
            });

            return CommandResult.Success();
        }
    }
}
