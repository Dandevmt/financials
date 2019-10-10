using Financials.Application.Errors;

namespace Financials.Application.Users.UseCases
{
    public class AddUserInput
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string EmailVerified { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }

        public void Validate()
        {
            ValidationError error = ValidationError.New();
            if (string.IsNullOrWhiteSpace(FirstName))
                error.AddError(nameof(FirstName), $"First Name is required");

            if (string.IsNullOrWhiteSpace(LastName))
                error.AddError(nameof(LastName), $"Last Name is required");

            error.ThrowIfError();

        }

    }


}
