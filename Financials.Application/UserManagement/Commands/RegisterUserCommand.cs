

using Financials.Application.CQRS;
using System.Linq;

namespace Financials.Application.UserManagement.Commands
{
    public class RegisterUserCommand : ICommand
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public bool ValidateOnly { get; set; }
        public string Password { get; set; }
        public string Password2 { get; set; }

        public bool Validate(out ValidationError error)
        {
            ValidationError errors = ValidationError.New();

            if (string.IsNullOrWhiteSpace(FirstName))
                errors.AddError(nameof(FirstName), $"First Name is required");

            if (string.IsNullOrWhiteSpace(LastName))
                errors.AddError(nameof(LastName), $"Last Name is required");

            if (string.IsNullOrWhiteSpace(Email))
                errors.AddError(nameof(Email), "Email is required");

            if (!string.IsNullOrWhiteSpace(Email) && Email.Count(e => e == '@') != 1)
                errors.AddError(nameof(Email), "Invalid Email Format");

            if (string.IsNullOrWhiteSpace(Password))
                errors.AddError(nameof(Password), $"Password is required");

            if (string.IsNullOrWhiteSpace(Password2))
                errors.AddError(nameof(Password2), $"Password is required");

            if (Password != null && Password2 != null && !Password.Equals(Password2))
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
