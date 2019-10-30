using Financials.Application.UserManagement.Codes;
using Financials.Application.Configuration;
using Financials.Application.Email;
using Financials.Application.UserManagement.Repositories;
using Financials.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Financials.Application.CQRS;
using System.Linq;

namespace Financials.Application.UserManagement.Commands
{
    public class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand>
    {
        private readonly IUserRepository userRepo;
        private readonly ICodeGenerator codeGenerator;
        private readonly IEmailSender emailSender;
        private readonly AppSettings appSettings;
        public ForgotPasswordCommandHandler(
            IUserRepository userRepo,
            ICodeGenerator codeGenerator,
            IEmailSender emailSender, 
            AppSettings appSettings)
        {
            this.userRepo = userRepo;
            this.codeGenerator = codeGenerator;
            this.emailSender = emailSender;
            this.appSettings = appSettings;
        }

        public async Task<CommandResult> Handle(ForgotPasswordCommand email)
        {
            var user = userRepo.Get(email.Email);
            if (user == null)
            {
                return CommandResult.Fail();
            }
            user.ValidationCodes = user.ValidationCodes.Where(v => v.Type != ValidationCodeType.PasswordReset).ToList();

            var code = new ValidationCode()
            {
                Code = codeGenerator.Generate(30),
                CreatedDate = DateTime.Now,
                Type = ValidationCodeType.PasswordReset,
            };
            user.ValidationCodes.Add(code);
            userRepo.Update(user);

            var em = new PasswordResetEmail()
            {
                To = user.Credentials.Email,
                Url = string.Format(appSettings.PasswordResetUrl, user.Id.ToString(), code.Code)
            };
            await emailSender.Send(em);
            return CommandResult.Success();
        }
    }
}
