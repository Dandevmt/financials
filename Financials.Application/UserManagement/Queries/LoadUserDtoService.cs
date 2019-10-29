using Financials.Application.Configuration;
using Financials.Application.UserManagement.Repositories;
using Financials.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.UserManagement.Queries
{
    public class LoadUserDtoService
    {
        private readonly AppSettings appSettings;
        private readonly IValidationCodeRepository codeRepo;
        private readonly ICredentialRepository credRepo;
        private readonly IUserRepository userRepo;

        public LoadUserDtoService(
            AppSettings appSettings,
            IValidationCodeRepository codeRepo,
            ICredentialRepository credRepo,
            IUserRepository userRepo)
        {
            this.appSettings = appSettings;
            this.codeRepo = codeRepo;
            this.credRepo = credRepo;
            this.userRepo = userRepo;
        }

        public UserDto LoadUser(Guid userId)
        {
            var user = userRepo.Get(userId);
            if (user == null)
            {
                return null;
            }

            var federationCode = codeRepo.Get(userId, Entities.ValidationCodeType.Federation);
            var creds = credRepo.Get(userId);

            var userDto = new UserDto()
            {
                Id = userId.ToString(),
                FirstName = user.Profile.FirstName,
                LastName = user.Profile.LastName,
                Email = creds?.Email,
                EmailVerified = creds?.EmailVerified,
                Permissions = user.Permissions ?? new HashSet<string>() { "ViewUsers", "AddUsers" },
                Registered = user.Registered,
                Address = new AddresssDto()
                {
                    City = user.Profile.Address.City,
                    Country = user.Profile.Address.Country,
                    State = user.Profile.Address.State,
                    Street = user.Profile.Address.Street,
                    Zip = user.Profile.Address.Zip
                },
                FederationCode = federationCode?.Code,
                FederationCodeExpiration = federationCode == null ? (DateTime?)null : (federationCode.CreatedDate.AddDays(appSettings.FederationCodeDurationDays))
            };
            return userDto;
        }
    }
}
