using Financials.CQRS;
using Financials.Application.Email;
using Financials.Application.UserManagement.Repositories;
using Financials.Application.UserManagement.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.UserManagement.Commands
{
    public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand>
    {
        private IUserRepository userRepo;
        private IPasswordHasher hasher;
        private IEmailSender emailSender;
        public ResetPasswordCommandHandler(
            IUserRepository userRepo,
            IPasswordHasher hasher,
            IEmailSender emailSender)
        {
            this.userRepo = userRepo;
            this.hasher = hasher;
            this.emailSender = emailSender;
        }

        public async Task<Result> Handle(ResetPasswordCommand input)
        {
            if (!input.Validate(out ValidationError error))
                return Result.Fail(error);

            if (input.ValidateOnly)
                return Result.Success();

            var userGuid = Guid.Parse(input.UserId);
            var user = userRepo.Get(userGuid, "tenant");
            if (user == null)
                return Result.Fail(UserManagementError.UserNotFound("User not found"));

            var code = user.ValidationCodes.FirstOrDefault(c => c.Type == Entities.ValidationCodeType.PasswordReset);
            var creds = user.Credentials;

            bool codesMatch = code != null && code.Code.Equals(input.ResetCode);
            bool passMatch = creds != null && hasher.VerifyPassword(creds.Password, input.NewPassword);

            if (!codesMatch && !passMatch)
            {
                return Result.Fail(UserManagementError.InvalidCodePasswordOrUserId());             
            }                

            // Creds shouldn't be null
            if (creds == null)
            {
                return Result.Fail(UserManagementError.InvalidCodePasswordOrUserId());
            }                

            creds.Password = hasher.HashPassword(input.NewPassword);

            userRepo.Update(user);

            await emailSender.Send(new PasswordWasResetEmail() 
            {
                To = creds.Email
            });

            return Result.Success();
        }
    }
}
