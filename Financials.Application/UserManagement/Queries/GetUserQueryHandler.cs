using Financials.Application.CQRS;
using Financials.Application.UserManagement.Security;
using Financials.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.UserManagement.Queries
{
    public class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserDto>
    {
        private readonly IAccess access;

        public GetUserQueryHandler(IAccess access)
        {
            this.access = access;
        }

        public Task<CommandResult<UserDto>> Handle(GetUserQuery query)
        {
            var user = access.CurrentUser();
            if (user == null)
                return CommandResult<UserDto>.Fail(UserManagementError.UserNotLoggedIn()).AsTaskTyped();

            var userDto = new UserDto() 
            {
                FirstName = user.Profile.FirstName,
                LastName = user.Profile.LastName,
                Permissions = user.Permissions ?? new HashSet<string>() { "ViewUsers", "AddUsers" },
                Address = new AddresssDto() 
                {
                    City = user.Profile.Address.City,
                    Country = user.Profile.Address.Country,
                    State = user.Profile.Address.State,
                    Street = user.Profile.Address.Street,
                    Zip = user.Profile.Address.Zip
                }
            };

            return CommandResult<UserDto>.Success(userDto).AsTaskTyped();
        }
    }

    public class GetUserQuery : IQuery<UserDto>
    {

    }
}
