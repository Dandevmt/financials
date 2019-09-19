using Financials.Application.Codes;
using Financials.Application.Errors;
using Financials.Application.Repositories;
using Financials.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.Users.UseCases
{
    public class RegisterUser : IUseCase<RegisterUserInput, User>
    {
        private readonly IUserRepository userRepo;
        private readonly IValidationCodeRepository codeRepo;
        private readonly ICodeGenerator codeGenerator;
        private readonly ICredentialRepository credRepo;
        private readonly IPasswordHasher hasher;
        public RegisterUser(
            IUserRepository userRepo, 
            IValidationCodeRepository codeRepo, 
            ICodeGenerator codeGenerator,
            ICredentialRepository credRepo,
            IPasswordHasher hasher)
        {
            this.userRepo = userRepo;
            this.codeRepo = codeRepo;
            this.codeGenerator = codeGenerator;
            this.credRepo = credRepo;
            this.hasher = hasher;
        }

        public void Handle(RegisterUserInput input, Action<User> presenter)
        {
            var creds = GetCredentials(input.Email);
            if (creds != null && string.IsNullOrWhiteSpace(input.FederationCode))
            {
                Error.EmailExists.Throw();
            }

            if (!string.IsNullOrWhiteSpace(input.FederationCode))
            {                
                if (creds != null)
                {
                    var federationCode = GetValidationCode(creds.UserId);
                    if (federationCode.Code != input.FederationCode)
                    {
                        // Invalid Code
                    } else if ((DateTime.Today - federationCode.CreatedDate).TotalDays > 15)
                    {
                        // Code is no longer valid - contact organization
                    } else
                    {
                        // Update User
                        var user = GetUser(creds.UserId);
                        user.Profile.FirstName = input.FirstName;
                        user.Profile.LastName = input.LastName;
                        user.Profile.EmailVerified = DateTime.Now;
                        user.Profile.Address.City = input.City;
                        user.Profile.Address.State = input.State;
                        user.Profile.Address.Country = input.Country;
                        user.Profile.Address.Street = input.Street;
                        user.Profile.Address.Zip = input.Zip;

                        codeRepo.Delete(creds.UserId, ValidationCodeType.Federation);
                    }
                } else
                {

                }
            } else
            {

            }

            presenter(null);        
        }

        private ValidationCode GetValidationCode(Guid userId)
        {
            return codeRepo.Get(userId, ValidationCodeType.Federation);
        }

        private Credentials GetCredentials(string email)
        {
            return credRepo.Get(email);
        }

        private User GetUser(Guid id)
        {
            return userRepo.Get(id);
        }
    }

    public class RegisterUserInput : AddUserInput
    {
        public string Password { get; set; }
        public string FederationCode { get; set; }
    }
}
