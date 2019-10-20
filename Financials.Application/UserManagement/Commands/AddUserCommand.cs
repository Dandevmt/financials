using Financials.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.UserManagement.Commands
{
    public class AddUserCommand : ICommand
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }

        public virtual bool Validate(out ValidationError errors)
        {
            ValidationError error = ValidationError.New();
            if (string.IsNullOrWhiteSpace(FirstName))
                error.AddError(nameof(FirstName), $"First Name is required");

            if (string.IsNullOrWhiteSpace(LastName))
                error.AddError(nameof(LastName), $"Last Name is required");

            if (error.HasError)
            {
                errors = error;
                return false;
            }
            errors = null;
            return true;            
        }
    }
}
