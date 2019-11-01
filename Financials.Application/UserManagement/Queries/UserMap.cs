using Financials.Dto;
using Financials.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Financials.Application.UserManagement.Queries
{
    public static class UserMap
    {
        public static UserForUserDto ToUserForUserDto(User user, Dictionary<string,string> tenantNames)
        {
            if (user == null) return null;
            var userDto = new UserForUserDto()
            {
                Id = user.Id.ToString(),
                FirstName = user.Profile.FirstName,
                LastName = user.Profile.LastName,
                Email = user.Credentials?.Email,
                EmailVerified = user.Credentials?.EmailVerified,
                Address = new AddresssDto()
                {
                    City = user.Profile.Address.City,
                    Country = user.Profile.Address.Country,
                    State = user.Profile.Address.State,
                    Street = user.Profile.Address.Street,
                    Zip = user.Profile.Address.Zip
                },
                Tenants = user.Tenants.Select(t => new UserTenantDto() 
                {
                    TenantId = t.TenantId,
                    Federated = t.Federated,
                    TenantName = tenantNames.FirstOrDefault(tn => tn.Key == t.TenantId).Value,
                    Permissions = t.Permissions
                }).ToList()
            };
            return userDto;
        }

        public static UserForTenantDto ToUserForTenantDto(User user, string tenantId)
        {
            if (user == null) return null;
            return new UserForTenantDto()
            {
                Id = user.Id.ToString(),
                FirstName = user.Profile.FirstName,
                LastName = user.Profile.LastName,
                Email = user.Credentials?.Email,
                EmailVerified = user.Credentials?.EmailVerified,
                Federated = user.Tenants.FirstOrDefault(t => t.TenantId == tenantId).Federated,
                FederationCode = user.Tenants.FirstOrDefault(t => t.TenantId == tenantId).FederationCode,
                Permissions = user.Tenants.FirstOrDefault(t => t.TenantId == tenantId).Permissions,
                Address = new AddresssDto()
                {
                    City = user.Profile.Address.City,
                    State = user.Profile.Address.State,
                    Street = user.Profile.Address.Street,
                    Country = user.Profile.Address.Country,
                    Zip = user.Profile.Address.Zip
                }
            };
        }
    }
}
