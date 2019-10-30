using Financials.Application.Configuration;
using Financials.Application.CQRS;
using Financials.Application.UserManagement.Repositories;
using Financials.Application.UserManagement.Security;
using Financials.Dto;
using Financials.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.UserManagement.Queries
{
    public class GetAllUsersQuery : IQuery<IEnumerable<UserDto>>
    {
    }

    [RequirePermission(Permission.ViewUsers)]
    public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUserRepository userRepo;
        private readonly AppSettings appSettings;
        public GetAllUsersQueryHandler(IUserRepository userRepo, AppSettings appSettings)
        {
            this.userRepo = userRepo;
            this.appSettings = appSettings;
        }
        public Task<CommandResult<IEnumerable<UserDto>>> Handle(GetAllUsersQuery query)
        {
            var users = userRepo.GetAll().Select(u => new UserDto() 
            {
                Id = u.Id.ToString(),
                FirstName = u.Profile.FirstName,
                LastName = u.Profile.LastName,
                Email = u.Credentials?.Email,
                EmailVerified = u.Credentials?.EmailVerified,    
                Registered = u.Registered,
                FederationCode = GetFedCode(u.ValidationCodes.ToList()),
                FederationCodeExpiration = GetFedCodeExpiration(u.ValidationCodes.ToList()),
                Permissions = u.Permissions,
                Address = new AddresssDto() 
                {
                    City = u.Profile.Address.City,
                    State = u.Profile.Address.State,
                    Street = u.Profile.Address.Street,
                    Country = u.Profile.Address.Country,
                    Zip = u.Profile.Address.Zip
                }
            });
            return CommandResult<IEnumerable<UserDto>>.Success(users).AsTaskTyped();
        }

        private string GetFedCode(IList<ValidationCode> codes)
        {
            if (codes == null)
                return null;

            var code = codes.FirstOrDefault(c => c.Type == ValidationCodeType.Federation);
            return code?.Code;
        }
        
        private DateTime? GetFedCodeExpiration(IList<ValidationCode> codes)
        {
            if (codes == null)
                return null;

            var code = codes.FirstOrDefault(c => c.Type == ValidationCodeType.Federation);
            return code?.CreatedDate.AddDays(appSettings.FederationCodeDurationDays);
        }
    }
}
