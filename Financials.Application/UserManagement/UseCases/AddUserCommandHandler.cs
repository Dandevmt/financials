using Financials.Application.CQRS;
using Financials.Application.UserManagement.Codes;
using Financials.Application.UserManagement.Repositories;
using Financials.Application.UserManagement.Security;
using Financials.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.UserManagement.UseCases
{
    [RequirePermission(Permission.AddUser)]
    public class AddUserCommandHandler : ICommandHandler<AddUserCommand>
    {
        private readonly IUserRepository userRepo;
        private readonly IValidationCodeRepository codeRepository;
        private readonly ICodeGenerator codeGenerator;
        private readonly IPasswordHasher hasher;
        private readonly ICredentialRepository credRepo;

        public AddUserCommandHandler(
            IUserRepository repo,
            IValidationCodeRepository codeRepository,
            ICodeGenerator codeGenerator,
            IPasswordHasher hasher,
            ICredentialRepository credRepo)
        {
            this.userRepo = repo;
            this.codeRepository = codeRepository;
            this.codeGenerator = codeGenerator;
            this.hasher = hasher;
            this.credRepo = credRepo;
        }

        public Task<CommandResult> Handle(AddUserCommand command)
        {
            if (!command.Validate(out ValidationError error))
                return CommandResult.Fail(error).AsTask();

            var user = AddUserFromInput(command);
            var creds = AddCredentialsIfEmail(command.Email, user.Id);
            var validationCode = AddValidationCodeIfNoEmail(user.Id, command.Email);

            return CommandResult<User>.Success(user).AsTask();
        }

        private User AddUserFromInput(AddUserCommand input)
        {
            var user = new User()
            {
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
            return userRepo.Add(user);
        }

        private Credentials AddCredentialsIfEmail(string email, Guid userId)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                var creds = new Credentials()
                {
                    UserId = userId,
                    Email = email,
                    Password = hasher.HashPassword(codeGenerator.Generate(30))
                };
                return credRepo.Add(creds);
            }
            else
            {
                return null;
            }
        }

        private ValidationCode AddValidationCodeIfNoEmail(Guid userId, string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return codeRepository.Add(new ValidationCode()
                {
                    UserId = userId,
                    CreatedDate = DateTime.Now,
                    Code = codeGenerator.Generate(8).ToUpper(),
                    Type = ValidationCodeType.Federation
                });
            }
            else
            {
                return null;
            }
        }
    }

}
