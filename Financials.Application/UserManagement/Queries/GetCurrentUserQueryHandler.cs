using Financials.Application.Configuration;
using Financials.CQRS;
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
    public class GetCurrentUserQueryHandler : IQueryHandler<GetCurrentUserQuery, UserForUserDto>
    {
        private readonly IAccess access;
        private readonly AppSettings appSettings;
        private readonly IUserRepository userRepo;
        private readonly ITenantRepository tenantRepo;

        public GetCurrentUserQueryHandler(
            IAccess access, 
            AppSettings appSettings, 
            IUserRepository userRepo,
            ITenantRepository tenantRepo)
        {
            this.access = access;
            this.appSettings = appSettings;
            this.userRepo = userRepo;
            this.tenantRepo = tenantRepo;
        }

        public Task<Result<UserForUserDto>> Handle(GetCurrentUserQuery query)
        {
            var user = access.CurrentUser();
            if (user == null)
                return Result<UserForUserDto>.Fail(UserManagementError.UserNotLoggedIn()).AsTaskTyped();

            var userDto = UserMap.ToUserForUserDto(user, tenantRepo.GetAll().ToDictionary(t => t.TenantId.ToString(), t => t.TenantName));

            if (userDto == null)
            {
                return Result<UserForUserDto>.Fail(UserManagementError.UserNotFound()).AsTaskTyped();
            }

            return Result<UserForUserDto>.Success(userDto).AsTaskTyped();
        }
    }

    public class GetCurrentUserQuery : IQuery<UserForUserDto>
    {
    }
}
