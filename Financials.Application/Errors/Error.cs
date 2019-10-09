using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.Errors
{
    public class Error
    {
        public ErrorCode Code { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        protected Error(ErrorCode code, string message)
        {
            Code = code;
            Message = message;
        }
        protected Error(ErrorCode code, string message, string description) : this(code, message)
        {            
            Description = description;
        }

        public void Throw(string description = null)
        {
            throw new ErrorException(new Error(Code, Message, description ?? Description));
        }

        public void Throw(Exception innerException, string description = null)
        {
            throw new ErrorException(new Error(Code, Message, description ?? Description), innerException);
        }

        public override string ToString()
        {
            return $"{{\"code\":{(int)Code},\"message\":\"{Message}\",\"description\":\"{Description}\"}}";
        }

        public static Error EmailExists = new Error(ErrorCode.Validation, "Email already exists", "Please enter a activation code, login, reset your password or contact the organization");
        public static Error EmailNotVerified = new Error(ErrorCode.EmailNotVerified, "Email not validated", "Please verify Email");
        public static Error EmailCouldNotUpdateDatabase = new Error(ErrorCode.EmailCouldNotUpdateDatabase, "Could not update database with Email");
        public static Error InvalidEmailOrPassword = new Error(ErrorCode.InvalidEmailOrPassword, "Invalid Email or Password");
        public static Error InvalidEmailVerificationCode = new Error(ErrorCode.InvalidEmailVerificationCode, "Invalid Email Verification Code");
        public static Error InvalidFederationCode = new Error(ErrorCode.InvalidFederationCode, "Invalid Federation Code");
        public static Error UserNotFound = new Error(ErrorCode.UserNotFound, "User Not Found");

    }
}
