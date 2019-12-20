using Financials.Application.Configuration;
using Financials.CQRS;
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
    public class GetAllUsersQuery : IQuery<IEnumerable<UserForTenantDto>>, IRequirePermission
    {
        public Permission Permission => Permission.ViewUsers;
        public string TenantId { get; set; }

        public GetAllUsersQuery(string tenantId)
        {
            TenantId = tenantId;
        }
    }


    public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, IEnumerable<UserForTenantDto>>
    {
        private readonly IUserRepository userRepo;

        public GetAllUsersQueryHandler(IUserRepository userRepo)
        {
            this.userRepo = userRepo;
        }
        public Task<Result<IEnumerable<UserForTenantDto>>> Handle(GetAllUsersQuery query)
        {
            var users = userRepo.GetAll(query.TenantId).Select(u => UserMap.ToUserForTenantDto(u, query.TenantId));
            return Result<IEnumerable<UserForTenantDto>>.Success(users).AsTaskTyped();
        }
    }
}
