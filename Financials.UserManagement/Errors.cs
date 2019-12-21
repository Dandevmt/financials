using Financials.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.UserManagement
{
    public class Errors : Error
    {
        protected Errors(int code, string description) : base(code, description)
        {
        }

        public static Error InvalidResetPasswordCode() => new Errors(111, "Invalid Password Reset Code");
        public static Error EmailExists() => new Errors(100, "Email already exists");
        public static Error EmailNotVerified() => new Errors(101, "Email not validated");
        public static Error EmailCouldNotUpdateDatabase() => new Errors(102, "Could not update database with Email");
        public static Error InvalidEmailOrPassword() => new Errors(103, "Invalid Email or Password");
        public static Error InvalidEmailVerificationCode() => new Errors(104, "Invalid Email Verification Code");
        public static Error InvalidFederationCode() => new Errors(105, "Invalid Federation Code");
        public static Error InvalidCodePasswordOrUserId() => new Errors(106, "Invalid Code, Password Or User Id");
        public static Error UserNotFound(string description = null) => new Errors(107, description ?? "User Not Found");
        public static Error UserNotLoggedIn() => new Errors(108, "User Not Logged In");
        public static Error TenanNotFound() => new Errors(109, "Tenant Not Found");
        public static Error TenantCodeAlreadyExists() => new Errors(110, "Tenant Code Already Exists");
    }
}
