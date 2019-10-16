using Financials.Application.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.UserManagement
{
    public class UserManagementError: Error
    {
        protected UserManagementError(ErrorCode code, string message) : base(code, message)
        {
        }

        public static Error EmailExists() => new UserManagementError(ErrorCode.Validation, "Email already exists");
        public static Error EmailNotVerified() => new UserManagementError(ErrorCode.EmailNotVerified, "Email not validated");
        public static Error EmailCouldNotUpdateDatabase() => new UserManagementError(ErrorCode.EmailCouldNotUpdateDatabase, "Could not update database with Email");
        public static Error InvalidEmailOrPassword() => new UserManagementError(ErrorCode.InvalidEmailOrPassword, "Invalid Email or Password");
        public static Error InvalidEmailVerificationCode() => new UserManagementError(ErrorCode.InvalidEmailVerificationCode, "Invalid Email Verification Code");
        public static Error InvalidFederationCode() => new UserManagementError(ErrorCode.InvalidFederationCode, "Invalid Federation Code");
        public static Error InvalidCodePasswordOrUserId() => new UserManagementError(ErrorCode.InvalidCodePasswordOrUserId, "Invalid Code, Password Or User Id");
        public static Error UserNotFound() => new UserManagementError(ErrorCode.UserNotFound, "User Not Found");
    }
}
