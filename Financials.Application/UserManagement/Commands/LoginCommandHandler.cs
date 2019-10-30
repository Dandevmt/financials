using Financials.Application.Configuration;
using Financials.Application.CQRS;
using Financials.Application.UserManagement.Repositories;
using Financials.Application.UserManagement.Security;
using Financials.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.UserManagement.Commands
{
    public class LoginCommandHandler : ICommandHandler<LoginCommand>
    {
        private readonly AppSettings appSettings;
        private readonly ITokenBuilder tokenBuilder;
        private readonly IUserRepository userRepo;
        private readonly IPasswordHasher hasher;

        public LoginCommandHandler(
            AppSettings appSettings,
            ITokenBuilder tokenBuilder, 
            IPasswordHasher hasher,
            IUserRepository userRepo)
        {
            this.appSettings = appSettings;
            this.userRepo = userRepo;
            this.tokenBuilder = tokenBuilder;
            this.hasher = hasher;
        }

        public Task<CommandResult> Handle(LoginCommand input)
        {
            var user = userRepo.Get(input.Email);
            if (user == null || user.Credentials == null)
                return CommandResult.Fail(UserManagementError.InvalidEmailOrPassword()).AsTask();

            if (user.Credentials.EmailVerified == null)
                return CommandResult.Fail(UserManagementError.EmailNotVerified()).AsTask();

            if (!hasher.VerifyPassword(user.Credentials.Password, input.Password))
                return CommandResult.Fail(UserManagementError.InvalidEmailOrPassword()).AsTask();

            var tokenDto = new TokenDto()
            {
                Token = tokenBuilder.Build(user, appSettings.TokenDurationMinutes),
                TokenDurationMinutes = appSettings.TokenDurationMinutes,
                Domain = appSettings.TokenDomain
            };
            return CommandResult<TokenDto>.Success(tokenDto).AsTask();
        }
    }
}
