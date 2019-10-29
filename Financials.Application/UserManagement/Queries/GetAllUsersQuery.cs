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
        private readonly IValidationCodeRepository codeRepo;
        private readonly ICredentialRepository credRepo;
        private readonly AppSettings appSettings;
        public GetAllUsersQueryHandler(IUserRepository userRepo, IValidationCodeRepository codeRepo, ICredentialRepository credRepo, AppSettings appSettings)
        {
            this.userRepo = userRepo;
            this.codeRepo = codeRepo;
            this.credRepo = credRepo;
            this.appSettings = appSettings;
        }
        public Task<CommandResult<IEnumerable<UserDto>>> Handle(GetAllUsersQuery query)
        {
            var fedCodes = codeRepo.GetAll(ValidationCodeType.Federation).ToDictionary(f => f.UserId, f => f);
            var creds = credRepo.GetAll().ToDictionary(c => c.UserId, c => c);

            var users = userRepo.GetAll().Select(u => new UserDto() 
            {
                Id = u.Id.ToString(),
                FirstName = u.Profile.FirstName,
                LastName = u.Profile.LastName,
                Email = GetEmail(creds, u.Id),
                EmailVerified = GetEmailVerified(creds, u.Id),
                Registered = u.Registered,
                FederationCode = GetFedCode(fedCodes, u.Id),
                FederationCodeExpiration = GetFedCodeExpiration(fedCodes, u.Id),
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

        private string GetFedCode(Dictionary<Guid, ValidationCode> codes, Guid userId)
        {
            if (codes.TryGetValue(userId, out ValidationCode code))
                return code.Code;
            return null;
        }
        
        private DateTime? GetFedCodeExpiration(Dictionary<Guid, ValidationCode> codes, Guid userId)
        {
            if (codes.TryGetValue(userId, out ValidationCode code))
                return code.CreatedDate.AddDays(appSettings.FederationCodeDurationDays);

            return null;
        }

        private string GetEmail(Dictionary<Guid, Credentials> creds, Guid userId)
        {
            if (creds.TryGetValue(userId, out Credentials cred))
                return cred.Email;

            return null;
        }

        private DateTime? GetEmailVerified(Dictionary<Guid, Credentials> creds, Guid userId)
        {
            if (creds.TryGetValue(userId, out Credentials cred))
                return cred.EmailVerified;

            return null;
        }
    }
}
