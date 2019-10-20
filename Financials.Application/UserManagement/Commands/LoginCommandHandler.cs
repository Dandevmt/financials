using Financials.Application.CQRS;
using Financials.Application.UserManagement.Repositories;
using Financials.Application.UserManagement.Security;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.UserManagement.Commands
{
    public class LoginCommandHandler : ICommandHandler<LoginCommand>
    {
        private readonly ITokenBuilder tokenBuilder;
        private readonly ICredentialRepository credRepo;
        private readonly IUserRepository userRepo;
        private readonly IPasswordHasher hasher;

        public LoginCommandHandler(
            ITokenBuilder tokenBuilder, 
            ICredentialRepository credRepo, 
            IPasswordHasher hasher,
            IUserRepository userRepo)
        {
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

            return CommandResult<string>.Success(tokenBuilder.Build(user)).AsTask();
        }
    }
}
