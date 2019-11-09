using Financials.Application.Configuration;
using Financials.Application.Email;
using Financials.Application.UserManagement.Codes;
using Financials.Application.UserManagement.Repositories;
using Financials.Application.UserManagement.Security;
using Financials.CQRS;
using Financials.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.UserManagement.Commands
{
    public class AddUserCommandHandler : ICommandHandler<AddUserCommand>
    {
        private readonly IUserRepository userRepo;
        private readonly ITenantRepository tenantRepository;
        private readonly ICodeGenerator codeGenerator;
        private readonly IPasswordHasher hasher;
        private readonly AppSettings appSettings;
        private readonly IEmailSender emailSender;

        public AddUserCommandHandler(
            IUserRepository repo,
            ITenantRepository tenantRepository,
            ICodeGenerator codeGenerator,
            IPasswordHasher hasher,
            AppSettings appSettings,
            IEmailSender emailSender)
        {
            this.userRepo = repo;
            this.tenantRepository = tenantRepository;
            this.codeGenerator = codeGenerator;
            this.hasher = hasher;
            this.appSettings = appSettings;
            this.emailSender = emailSender;
        }

        public async Task<CommandResult> Handle(AddUserCommand command)
        {
            if (!command.Validate(out ValidationError error))
                return CommandResult.Fail(error);

            if (!string.IsNullOrWhiteSpace(command.Email) && userRepo.Get(command.Email) != null)
                return CommandResult.Fail(ValidationError.New().AddError(nameof(command.Email), "Email Already Exists"));

            if (tenantRepository.Get(command.TenantId) == null)
                return CommandResult.Fail(UserManagementError.TenanNotFound());

            if (command.ValidateOnly)
                return CommandResult.Success();            

            var user = GetUserFromInput(command);
            user.Credentials = GetCredentialsIfEmail(command.Email);
            user.ValidationCodes = SetupValidationCodes(command);
            userRepo.Add(user);

            if (!string.IsNullOrWhiteSpace(command.Email))
            {
                var email = new VerifyEmailEmail()
                {
                    To = command.Email,
                    Url = string.Format(appSettings.EmailVerificationUrl, user.Id.ToString(), user.ValidationCodes.FirstOrDefault().Code)
                };
                await emailSender.Send(email);
            }            

            return CommandResult<string>.Success(user.Id.ToString());
        }

        private User GetUserFromInput(AddUserCommand input)
        {
            var user = new User()
            {
                Tenants = new List<UserTenant> 
                { 
                    new UserTenant()
                    {
                        TenantId = input.TenantId,
                        FederationCode = codeGenerator.Generate(10)
                    }
                },
                Profile = new UserProfile()
                {
                    FirstName = input.FirstName,
                    LastName = input.LastName,
                    Address = new Address()
                    {
                        City = input.City,
                        State = input.State,
                        Zip = input.Zip,
                        Street = input.Street,
                        Country = input.Country
                    }
                }
            };

            return user;
        }

        private ICollection<ValidationCode> SetupValidationCodes(AddUserCommand command)
        {
            List<ValidationCode> codes = new List<ValidationCode>();
            if (!string.IsNullOrWhiteSpace(command.Email))
            {
                codes.Add(new ValidationCode()
                {
                    CreatedDate = DateTime.Now,
                    Code = codeGenerator.Generate(20),
                    Type = ValidationCodeType.Email
                });
            }
            return codes;
        }

        private Credentials GetCredentialsIfEmail(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                var creds = new Credentials()
                {
                    Email = email,
                    Password = hasher.HashPassword(codeGenerator.Generate(30))
                };
                return creds;
            }
            else
            {
                return null;
            }
        }
    }

}
