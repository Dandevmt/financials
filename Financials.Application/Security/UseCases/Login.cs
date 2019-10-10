using Financials.Application.Errors;
using Financials.Application.Repositories;
using Financials.Application.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.Security.UseCases
{
    public class Login : IUseCase<LoginInput, string>
    {
        private readonly ITokenBuilder tokenBuilder;
        private readonly ICredentialRepository credRepo;
        private readonly IUserRepository userRepo;
        private readonly IPasswordHasher hasher;

        public Login(
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

        public async Task Handle(LoginInput input, Action<string> presenter)
        {
            var creds = credRepo.Get(input.Email);
            if (creds == null)
                Error.InvalidEmailOrPassword().Throw();

            if (creds.EmailVerified == null)
                Error.EmailNotVerified().Throw();

            if (!hasher.VerifyPassword(creds.Password, input.Password))
                Error.InvalidEmailOrPassword().Throw();

            var user = userRepo.Get(creds.UserId);
            if (user == null)
                Error.UserNotFound().Throw($"User with id of {creds.UserId.ToString()} was not found");

            presenter(tokenBuilder.Build(user));
        }
    }
}
