using Financials.Application.Configuration;
using Financials.Application.UserManagement.Repositories;
using Financials.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Financials.Application.UserManagement.Queries
{
    public class LoadUserDtoService
    {
        private readonly AppSettings appSettings;
        private readonly IUserRepository userRepo;

        public LoadUserDtoService(
            AppSettings appSettings,
            IUserRepository userRepo)
        {
            this.appSettings = appSettings;
            this.userRepo = userRepo;
        }

        public UserDto LoadUser(Guid userId)
        {
            var user = userRepo.Get(userId);
            if (user == null)
            {
                return null;
            }

            var fedCode = user.ValidationCodes.FirstOrDefault(c => c.Type == Entities.ValidationCodeType.Federation);

            var userDto = new UserDto()
            {
                Id = userId.ToString(),
                FirstName = user.Profile.FirstName,
                LastName = user.Profile.LastName,
                Email = user.Credentials?.Email,
                EmailVerified = user.Credentials?.EmailVerified,
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
                FederationCode = fedCode?.Code,
                FederationCodeExpiration = fedCode == null ? (DateTime?)null : (fedCode.CreatedDate.AddDays(appSettings.FederationCodeDurationDays))
            };
            return userDto;
        }
    }
}
