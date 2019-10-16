using Financials.Application.Email;
using Financials.Application.Repositories;
using Financials.Application.Security;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.Users.UseCases
{
    public class ResetPassword : IUseCase<ResetPasswordInput, bool>
    {
        private IValidationCodeRepository codeRepo;
        private ICredentialRepository credRepo;
        private IPasswordHasher hasher;
        private IEmailSender emailSender;
        public ResetPassword(
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

        public async Task Handle(ResetPasswordInput input, Action<bool> presenter)
        {
            input.Validate();

            var code = codeRepo.Get(input.UserId, Entities.ValidationCodeType.PasswordReset);
            var creds = credRepo.Get(input.UserId);

            bool codesMatch = code != null && code.Code.Equals(input.ResetCode);
            bool passMatch = creds != null && hasher.VerifyPassword(creds.Password, input.NewPassword);

            if (!codesMatch && !passMatch)
            {
                presenter(false);
                Errors.Error.InvalidCodePasswordOrUserId().Throw();                
            }                

            // Creds shouldn't be null
            if (creds == null)
            {
                presenter(false);
                Errors.Error.InvalidCodePasswordOrUserId().Throw();
            }                

            creds.Password = hasher.HashPassword(input.NewPassword);

            credRepo.UpdateOne(creds);

            await emailSender.Send(new PasswordWasResetEmail() 
            {
                To = creds.Email
            });

            presenter(true);
        }
    }
}
