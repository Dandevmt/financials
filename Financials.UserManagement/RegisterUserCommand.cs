using Financials.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financials.UserManagement
{
    public class RegisterUserCommand : ICommand<string>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Email2 { get; set; }
        public bool ValidateOnly { get; set; }
        public string Password { get; set; }
        public string Password2 { get; set; }

        public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, string>
        {
            private readonly IUserRepository userRepo;
            private readonly ICodeGenerator codeGenerator;
            private readonly IHasherService hasher;
            private readonly Dispatcher dispatcher;

            public RegisterUserCommandHandler(
                IUserRepository userRepo,
                ICodeGenerator codeGenerator,
                IHasherService hasher,
                Dispatcher dispatcher)
            {
                this.userRepo = userRepo;
                this.codeGenerator = codeGenerator;
                this.hasher = hasher;
                this.dispatcher = dispatcher;
            }

            public Result<string> Handle(RegisterUserCommand input)
            {
                var result = Result<string>.Success("0");

                if (!string.IsNullOrWhiteSpace(input.Email) && userRepo.GetByUsername(input.Username) != null)
                    result = Result<string>.Fail(null, ValidationError.New().AddError(nameof(input.Username), "Ussername Already Exists"));

                var emailR = UserManagement.Email.Create(input.Email, input.Email2, codeGenerator);
                var passwordR = PasswordHashed.Create(input.Password, input.Password2, hasher);
                var userR = User.Create(input.Username, emailR.Value, passwordR.Value);

                result = result.Merge(emailR, passwordR, userR);
                if (!result.IsSuccess)
                    return result;

                var user = userRepo.Add(userR.Value);

                dispatcher.QueueEvent(new RegisterUserEvent(input.Email, input.Username, emailR.Value.VerificationCode));

                return Result<string>.Success(user.Id.ToString());
            }
        }
    }
}
