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
    public class GetCurrentUserQueryHandler : IQueryHandler<GetCurrentUserQuery, UserDto>
    {
        private readonly IAccess access;
        private readonly AppSettings appSettings;
        private readonly IValidationCodeRepository codeRepo;
        private readonly ICredentialRepository credRepo;
        private readonly IUserRepository userRepo;

        public GetCurrentUserQueryHandler(
            IAccess access, 
            AppSettings appSettings, 
            IValidationCodeRepository codeRepo, 
            ICredentialRepository credRepo,
            IUserRepository userRepo)
        {
            this.access = access;
            this.appSettings = appSettings;
            this.codeRepo = codeRepo;
            this.credRepo = credRepo;
            this.userRepo = userRepo;
        }

        public Task<CommandResult<UserDto>> Handle(GetCurrentUserQuery query)
        {
            var user = access.CurrentUser();
            if (user == null)
                return CommandResult<UserDto>.Fail(UserManagementError.UserNotLoggedIn()).AsTaskTyped();

            var userDto = new LoadUserDtoService(appSettings, codeRepo, credRepo, userRepo).LoadUser(user.Id);

            if (userDto == null)
            {
                return CommandResult<UserDto>.Fail(UserManagementError.UserNotFound()).AsTaskTyped();
            }
            // TODO: Remove this line
            userDto.Permissions = new HashSet<string>() { Permission.AddUsers.ToString(), Permission.ViewUsers.ToString(), Permission.EditUsers.ToString() };
            return CommandResult<UserDto>.Success(userDto).AsTaskTyped();
        }
    }

    public class GetCurrentUserQuery : IQuery<UserDto>
    {
    }
}
