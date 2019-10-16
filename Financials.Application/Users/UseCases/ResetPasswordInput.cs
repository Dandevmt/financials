﻿using Financials.Application.Errors;
using System;

namespace Financials.Application.Users.UseCases
{
    public class ResetPasswordInput
    {
        public Guid UserId { get; set; }
        public string ResetCode { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPassword2 { get; set; }

        public void Validate()
        {
            ValidationError error = ValidationError.New();
            if (string.IsNullOrWhiteSpace(OldPassword) && string.IsNullOrWhiteSpace(ResetCode))
            {
                error.AddError(nameof(OldPassword), $"Old Password or Reset Code is required");
                error.AddError(nameof(ResetCode), "Reset Code or Old Password is required");
            }

            if (UserId == null || UserId == Guid.Empty)
                error.AddError(nameof(UserId), "UserId is required");                

            if (string.IsNullOrWhiteSpace(NewPassword))
                error.AddError(nameof(NewPassword), $"New Password is required");

            if (string.IsNullOrWhiteSpace(NewPassword2))
                error.AddError(nameof(NewPassword2), $"New Password 2 is required");

            if (!NewPassword.Equals(NewPassword2))
                error.AddError(nameof(NewPassword2), $"Passwords must match");

            error.ThrowIfError();

        }
    }
}