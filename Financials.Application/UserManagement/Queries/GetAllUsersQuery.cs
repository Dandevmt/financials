using Financials.Application.CQRS;
using Financials.Application.UserManagement.Repositories;
using Financials.Application.UserManagement.Security;
using Financials.Dto;
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
        public GetAllUsersQueryHandler(IUserRepository userRepo)
        {
            this.userRepo = userRepo;
        }
        public Task<CommandResult<IEnumerable<UserDto>>> Handle(GetAllUsersQuery query)
        {
            var users = userRepo.GetAll().Select(u => new UserDto() 
            {
                FirstName = u.Profile.FirstName,
                LastName = u.Profile.LastName,
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
    }
}
