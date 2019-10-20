using Financials.Application.CQRS;
using Financials.Application.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.UserManagement
{
    public class UserManagementError: CommandError
    {
        protected UserManagementError(int code, int httpStatusCode, string description) : base(code, httpStatusCode, description)
        {
        }

        public static CommandError EmailExists() => new UserManagementError(100, 400, "Email already exists");
        public static CommandError EmailNotVerified() => new UserManagementError(101, 400, "Email not validated");
        public static CommandError EmailCouldNotUpdateDatabase() => new UserManagementError(102, 400, "Could not update database with Email");
        public static CommandError InvalidEmailOrPassword() => new UserManagementError(103, 400, "Invalid Email or Password");
        public static CommandError InvalidEmailVerificationCode() => new UserManagementError(104, 400, "Invalid Email Verification Code");
        public static CommandError InvalidFederationCode() => new UserManagementError(105, 400, "Invalid Federation Code");
        public static CommandError InvalidCodePasswordOrUserId() => new UserManagementError(106, 400, "Invalid Code, Password Or User Id");
        public static CommandError UserNotFound(string description = null) => new UserManagementError(107, 400, description ?? "User Not Found");
    }
}
