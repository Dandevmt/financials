using Financials.Application.Configuration;
using Financials.CQRS;
using Financials.Application.UserManagement.Repositories;
using Financials.Application.UserManagement.Security;
using Financials.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.UserManagement.Queries
{
    public class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserForTenantDto>
    {
        private readonly AppSettings appSettings;
        private readonly IUserRepository userRepo;

        public GetUserQueryHandler(
            AppSettings appSettings,
            IUserRepository userRepo)
        {
            this.appSettings = appSettings;
            this.userRepo = userRepo;
        }

        public Task<Result<UserForTenantDto>> Handle(GetUserQuery query)
        {
            if (string.IsNullOrEmpty(query.UserId))
                return Result<UserForTenantDto>.Fail(ValidationError.New().AddError(nameof(query.UserId), "User Id is required")).AsTaskTyped();

            if (!Guid.TryParse(query.UserId, out Guid userId))
                return Result<UserForTenantDto>.Fail(ValidationError.New().AddError(nameof(query.UserId), "User Id is not in a correct format")).AsTaskTyped();

            var user = userRepo.Get(userId, query.TenantId);

            var userDto = UserMap.ToUserForTenantDto(user, query.TenantId);

            if (userDto == null)
            {
                return Result<UserForTenantDto>.Fail(UserManagementError.UserNotFound()).AsTaskTyped();
            }

            return Result<UserForTenantDto>.Success(userDto).AsTaskTyped();
        }
    }

    public class GetUserQuery : IQuery<UserForTenantDto>, IRequirePermission
    {
        public Permission Permission => Permission.ViewUsers;
        public string UserId { get; set; }
        public string TenantId { get; set; }

        public GetUserQuery(string userId, string tenantId)
        {
            UserId = userId;
            TenantId = tenantId;
        }
    }
}
