using Financials.Application.UserManagement.Codes;
using Financials.Application.Configuration;
using Financials.Application.Email;
using Financials.Application.UserManagement.Repositories;
using Financials.Application.UserManagement.Security;
using Financials.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Financials.CQRS;

namespace Financials.Application.UserManagement.Commands
{
    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
    {
        private readonly IUserRepository userRepo;
        private readonly ICodeGenerator codeGenerator;
        private readonly IPasswordHasher hasher;
        private readonly IEmailSender emailSender;
        private readonly AppSettings appSettings;
        public RegisterUserCommandHandler(
            IUserRepository userRepo, 
            ICodeGenerator codeGenerator,
            IPasswordHasher hasher,
            IEmailSender emailSender,
            AppSettings appSettings)
        {
            this.userRepo = userRepo;
            this.codeGenerator = codeGenerator;
            this.hasher = hasher;
            this.emailSender = emailSender;
            this.appSettings = appSettings;
        }

        public async Task<CommandResult> Handle(RegisterUserCommand input)
        {
            if (!input.Validate(out ValidationError error))
                return CommandResult.Fail(error);

            if (!string.IsNullOrWhiteSpace(input.Email) && userRepo.Get(input.Email) != null)
                return CommandResult.Fail(ValidationError.New().AddError(nameof(input.Email), "Email Already Exists"));

            if (input.ValidateOnly)
                return CommandResult.Success();

            var user = new User() 
            {
                Registered = true,                
                Credentials = new Credentials() 
                {
                    Email = input.Email,
                    EmailVerified = null,
                    Password = hasher.HashPassword(input.Password)
                },
                Profile = new UserProfile() 
                {
                    FirstName = input.FirstName,
                    LastName = input.LastName,
                    Address = new Address() 
                    {
                        City = input.City,
                        State = input.State,
                        Street = input.Street,
                        Country = input.Country,
                        Zip = input.Zip
                    }
                },
                Tenants = new List<UserTenant>(),
                ValidationCodes = new List<ValidationCode>()
            };

            var emailCode = new ValidationCode()
            {
                Code = codeGenerator.Generate(30),
                CreatedDate = DateTime.Now,
                Type = ValidationCodeType.Email
            };

            user.ValidationCodes.Add(emailCode);
            userRepo.Add(user);

            var email = new VerifyEmailEmail()
            {
                To = input.Email,
                Url = string.Format(appSettings.EmailVerificationUrl, user.Id.ToString(), emailCode.Code)
            };
            await emailSender.Send(email);

            return CommandResult<string>.Success(user.Id.ToString());
        }
    }
}
