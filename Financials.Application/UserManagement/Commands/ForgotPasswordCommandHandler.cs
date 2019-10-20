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

namespace Financials.Application.UserManagement.Commands
{
    public class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand>
    {
        private readonly ICredentialRepository credRepo;
        private readonly IValidationCodeRepository codeRepo;
        private readonly ICodeGenerator codeGenerator;
        private readonly IEmailSender emailSender;
        private readonly AppSettings appSettings;
        public ForgotPasswordCommandHandler(
            ICredentialRepository credRepo, 
            IValidationCodeRepository codeRepo, 
            ICodeGenerator codeGenerator,
            IEmailSender emailSender, 
            AppSettings appSettings)
        {
            this.credRepo = credRepo;
            this.codeGenerator = codeGenerator;
            this.emailSender = emailSender;
            this.appSettings = appSettings;
            this.codeRepo = codeRepo;
        }

        public async Task<CommandResult> Handle(ForgotPasswordCommand email)
        {
            var creds = credRepo.Get(email.Email);
            if (creds == null)
            {
                return CommandResult.Fail();
            }

            var code = new ValidationCode()
            {
                Code = codeGenerator.Generate(30),
                CreatedDate = DateTime.Now,
                Type = ValidationCodeType.PasswordReset,
                UserId = creds.UserId
            };
            codeRepo.Add(code);

            var em = new PasswordResetEmail()
            {
                To = creds.Email,
                Url = string.Format(appSettings.PasswordResetUrl, creds.UserId.ToString(), code.Code)
            };
            await emailSender.Send(em);
            return CommandResult.Success();
        }
    }
}
