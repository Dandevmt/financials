using Financials.Application.Codes;
using Financials.Application.Repositories;
using Financials.Application.Security;
using Financials.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.Users.UseCases
{
    public class AddUser : IUseCase<AddUserInput, User>, IPermissionRequired
    {
        private readonly IUserRepository userRepo;
        private readonly IValidationCodeRepository codeRepository;
        private readonly ICodeGenerator codeGenerator;
        private readonly IPasswordHasher hasher;
        private readonly ICredentialRepository credRepo;

        public Permission PermissionRequired { get; private set; } = Permission.AddUser;

        public AddUser(IUserRepository repo, IValidationCodeRepository codeRepository, ICodeGenerator codeGenerator, IPasswordHasher hasher, ICredentialRepository credRepo)
        {
            this.userRepo = repo;
            this.codeRepository = codeRepository;
            this.codeGenerator = codeGenerator;
            this.hasher = hasher;
            this.credRepo = credRepo;
        }

        public void Handle(AddUserInput input, Action<User> presenter)
        {
            var user = AddUserFromInput(input);
            var creds = AddCredentialsIfEmail(input.Email, user.Id);
            var validationCode = AddValidationCodeIfNoEmail(user.Id, input.Email);
            presenter(user);
        }

        private User AddUserFromInput(AddUserInput input)
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
            } else
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
            } else
            {
                return null;
            }
        }
    }

}
