using Financials.Application.UserManagement.Codes;
using Financials.Application.Configuration;
using Financials.Application.Email;
using Financials.Application.UserManagement.Repositories;
using Financials.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.UserManagement.UseCases
{
    public class ForgotPassword : IUseCase<string, bool>
    {
        private readonly ICredentialRepository credRepo;
        private readonly IValidationCodeRepository codeRepo;
        private readonly ICodeGenerator codeGenerator;
        private readonly IEmailSender emailSender;
        private readonly AppSettings appSettings;
        public ForgotPassword(
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

        public async Task Handle(string email, Action<bool> presenter)
        {
            var creds = credRepo.Get(email);
            if (creds == null)
            {
                presenter(false);
                return;
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
            presenter(true);
        }
    }
}
