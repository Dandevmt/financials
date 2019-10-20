

using Financials.Application.CQRS;

namespace Financials.Application.UserManagement.Commands
{
    public class RegisterUserCommand : AddUserCommand
    {
        public string Password { get; set; }
        public string Password2 { get; set; }
        public string FederationCode { get; set; }

        public override bool Validate(out ValidationError error)
        {
            ValidationError errors;
            base.Validate(out errors);
            errors = errors ?? ValidationError.New();

            if (string.IsNullOrWhiteSpace(FederationCode))
                errors.AddError(nameof(FederationCode), "Federation Code is required");

            if (string.IsNullOrWhiteSpace(Password))
                errors.AddError(nameof(Password), $"Password is required");

            if (!Password.Equals(Password2))
                errors.AddError(nameof(Password2), "Passwords must match");

            if (errors.HasError)
            {
                error = errors;
                return false;
            }

            error = null;
            return true;
        }
    }
}
