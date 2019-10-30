using Financials.Application.CQRS;
using Financials.Application.UserManagement.Codes;
using Financials.Application.UserManagement.Repositories;
using Financials.Application.UserManagement.Security;
using Financials.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.UserManagement.Commands
{
    [RequirePermission(Permission.AddUsers)]
    public class AddUserCommandHandler : ICommandHandler<AddUserCommand>
    {
        private readonly IUserRepository userRepo;
        private readonly ICodeGenerator codeGenerator;
        private readonly IPasswordHasher hasher;

        public AddUserCommandHandler(
            IUserRepository repo,
            ICodeGenerator codeGenerator,
            IPasswordHasher hasher)
        {
            this.userRepo = repo;
            this.codeGenerator = codeGenerator;
            this.hasher = hasher;
        }

        public Task<CommandResult> Handle(AddUserCommand command)
        {
            if (!command.Validate(out ValidationError error))
                return CommandResult.Fail(error).AsTask();

            if (command.ValidateOnly)
                return CommandResult.Success().AsTask();

            var user = GetUserFromInput(command);
            user.Credentials = GetCredentialsIfEmail(command.Email);
            var validationCode = GetValidationCodeIfNoEmail(command.Email);
            user.ValidationCodes.Add(validationCode);
            userRepo.Add(user);

            return CommandResult<string>.Success(user.Id.ToString()).AsTask();
        }

        private User GetUserFromInput(AddUserCommand input)
        {
            var user = new User()
            {
                TenantIds = new List<string> { input.TenantId },
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
                },
                ValidationCodes = new List<ValidationCode>()
            };
            userRepo.Add(user);
            return user;
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

        private ValidationCode GetValidationCodeIfNoEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new ValidationCode()
                {
                    CreatedDate = DateTime.Now,
                    Code = codeGenerator.Generate(8).ToUpper(),
                    Type = ValidationCodeType.Federation
                };
            }
            else
            {
                return null;
            }
        }
    }

}
