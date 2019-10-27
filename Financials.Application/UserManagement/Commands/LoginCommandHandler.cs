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
        private readonly ICredentialRepository credRepo;
        private readonly IUserRepository userRepo;
        private readonly IPasswordHasher hasher;

        public LoginCommandHandler(
            AppSettings appSettings,
            ITokenBuilder tokenBuilder, 
            ICredentialRepository credRepo, 
            IPasswordHasher hasher,
            IUserRepository userRepo)
        {
            this.appSettings = appSettings;
            this.credRepo = credRepo;
            this.userRepo = userRepo;
            this.tokenBuilder = tokenBuilder;
            this.hasher = hasher;
        }

        public Task<CommandResult> Handle(LoginCommand input)
        {
            var creds = credRepo.Get(input.Email);
            if (creds == null)
                return CommandResult.Fail(UserManagementError.InvalidEmailOrPassword()).AsTask();

            if (creds.EmailVerified == null)
                return CommandResult.Fail(UserManagementError.EmailNotVerified()).AsTask();

            if (!hasher.VerifyPassword(creds.Password, input.Password))
                return CommandResult.Fail(UserManagementError.InvalidEmailOrPassword()).AsTask();

            var user = userRepo.Get(creds.UserId);
            if (user == null)
                return CommandResult.Fail(UserManagementError.UserNotFound($"User with id of {creds.UserId.ToString()} was not found")).AsTask();

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
