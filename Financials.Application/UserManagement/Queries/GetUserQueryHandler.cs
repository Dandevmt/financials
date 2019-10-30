using Financials.Application.Configuration;
using Financials.Application.CQRS;
using Financials.Application.UserManagement.Repositories;
using Financials.Application.UserManagement.Security;
using Financials.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.UserManagement.Queries
{
    [RequirePermission(Permission.ViewUsers)]
    public class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserDto>
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

        public Task<CommandResult<UserDto>> Handle(GetUserQuery query)
        {
            if (string.IsNullOrEmpty(query.UserId))
                return CommandResult<UserDto>.Fail(ValidationError.New().AddError(nameof(query.UserId), "User Id is required")).AsTaskTyped();

            if (!Guid.TryParse(query.UserId, out Guid userId))
                return CommandResult<UserDto>.Fail(ValidationError.New().AddError(nameof(query.UserId), "User Id is not in a correct format")).AsTaskTyped();

            var userDto = new LoadUserDtoService(appSettings, userRepo).LoadUser(userId);

            if (userDto == null)
            {
                return CommandResult<UserDto>.Fail(UserManagementError.UserNotFound()).AsTaskTyped();
            }

            return CommandResult<UserDto>.Success(userDto).AsTaskTyped();
        }
    }

    public class GetUserQuery : IQuery<UserDto>
    {
        public string UserId { get; set; }

        public GetUserQuery(string userId)
        {
            UserId = userId;
        }
    }
}
