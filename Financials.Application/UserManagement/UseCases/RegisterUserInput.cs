using Financials.Application.Errors;

namespace Financials.Application.UserManagement.UseCases
{
    public class RegisterUserInput : AddUserInput
    {
        public string Password { get; set; }
        public string Password2 { get; set; }
        public string FederationCode { get; set; }

        public override void Validate()
        {
            base.Validate();

            ValidationError error = ValidationError.New();

            if (string.IsNullOrWhiteSpace(FederationCode))
                error.AddError(nameof(FederationCode), "Federation Code is required");

            if (string.IsNullOrWhiteSpace(Password))
                error.AddError(nameof(Password), $"Password is required");

            if (!Password.Equals(Password2))
                error.AddError(nameof(Password2), "Passwords must match");

            error.ThrowIfError();

        }
    }
}
