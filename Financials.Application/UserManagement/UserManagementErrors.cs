using Financials.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.UserManagement
{
    public class UserManagementError: Error
    {
        protected UserManagementError(int code, string description) : base(code, description)
        {
        }

        public static Error EmailExists() => new UserManagementError(100, "Email already exists");
        public static Error EmailNotVerified() => new UserManagementError(101, "Email not validated");
        public static Error EmailCouldNotUpdateDatabase() => new UserManagementError(102, "Could not update database with Email");
        public static Error InvalidEmailOrPassword() => new UserManagementError(103, "Invalid Email or Password");
        public static Error InvalidEmailVerificationCode() => new UserManagementError(104, "Invalid Email Verification Code");
        public static Error InvalidFederationCode() => new UserManagementError(105, "Invalid Federation Code");
        public static Error InvalidCodePasswordOrUserId() => new UserManagementError(106, "Invalid Code, Password Or User Id");
        public static Error UserNotFound(string description = null) => new UserManagementError(107, description ?? "User Not Found");
        public static Error UserNotLoggedIn() => new UserManagementError(108, "User Not Logged In");
        public static Error TenanNotFound() => new UserManagementError(109, "Tenant Not Found");
        public static Error TenantCodeAlreadyExists() => new UserManagementError(110, "Tenant Code Already Exists");
    }
}
