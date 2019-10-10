using Financials.Application.Codes;
using Financials.Application.Configuration;
using Financials.Application.Email;
using Financials.Application.Errors;
using Financials.Application.Repositories;
using Financials.Application.Security;
using Financials.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.Users.UseCases
{
    public class RegisterUser : IUseCase<RegisterUserInput, User>
    {
        private readonly IUserRepository userRepo;
        private readonly IValidationCodeRepository codeRepo;
        private readonly ICodeGenerator codeGenerator;
        private readonly ICredentialRepository credRepo;
        private readonly IPasswordHasher hasher;
        private readonly IEmailSender emailSender;
        private readonly AppSettings appSettings;
        public RegisterUser(
            IUserRepository userRepo, 
            IValidationCodeRepository codeRepo, 
            ICodeGenerator codeGenerator,
            ICredentialRepository credRepo,
            IPasswordHasher hasher,
            IEmailSender emailSender,
            AppSettings appSettings)
        {
            this.userRepo = userRepo;
            this.codeRepo = codeRepo;
            this.codeGenerator = codeGenerator;
            this.credRepo = credRepo;
            this.hasher = hasher;
            this.emailSender = emailSender;
            this.appSettings = appSettings;
        }

        public async Task Handle(RegisterUserInput input, Action<User> presenter)
        {
            if (string.IsNullOrWhiteSpace(input.FederationCode))
                Error.InvalidFederationCode().Throw();

            var creds = GetCredentials(input.Email);
            if (creds != null)
                Error.EmailExists().Throw();

            var federationCode = GetValidationCode(input.FederationCode);
            if (federationCode == null || federationCode.Code != input.FederationCode || (DateTime.Today - federationCode.CreatedDate).TotalDays > 15)
            {
                Error.InvalidFederationCode().Throw();
            }
            else
            {
                // Update User
                var user = GetUser(federationCode.UserId);
                user.Profile.FirstName = input.FirstName;
                user.Profile.LastName = input.LastName;
                user.Profile.Address.City = input.City;
                user.Profile.Address.State = input.State;
                user.Profile.Address.Country = input.Country;
                user.Profile.Address.Street = input.Street;
                user.Profile.Address.Zip = input.Zip;

                credRepo.Add(new Credentials() 
                {
                    UserId = federationCode.UserId,
                    Email = input.Email,
                    EmailVerified = null,
                    Password = hasher.HashPassword(input.Password)
                });

                var emailCode = codeRepo.Add(new ValidationCode() 
                {
                    Code = codeGenerator.Generate(30),
                    CreatedDate = DateTime.Now,
                    Type = ValidationCodeType.Email,
                    UserId = federationCode.UserId
                });

                codeRepo.Delete(federationCode.UserId, ValidationCodeType.Federation);

                var email = new VerifyEmailEmail()
                {
                    To = input.Email,
                    Url = string.Format(appSettings.EmailVerificationUrl, federationCode.UserId.ToString(), emailCode.Code)
                };
                await emailSender.Send(email);

                presenter(user);
            }

            presenter(null);                  
        }

        private ValidationCode GetValidationCode(string federationCode)
        {
            return codeRepo.GetFederationCode(federationCode);
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
}
