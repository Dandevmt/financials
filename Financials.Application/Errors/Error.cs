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
        private Error(ErrorCode code, string message)
        {
            Code = code;
            Message = message;
        }
        private Error(ErrorCode code, string message, string description) : this(code, message)
        {            
            Description = description;
        }

        public void Throw(string description = null)
        {
            throw new ErrorException(Code, Message, description ?? Description);
        }

        public void Throw(Exception innerException, string description = null)
        {
            throw new ErrorException(Code, Message, description ?? Description, innerException);
        }

        public static Error EmailExists = new Error(ErrorCode.Validation, "Email already exists", "Please enter a activation code, login, reset your password or contact the organization");
        public static Error InvalidEmailOrPassword = new Error(ErrorCode.InvalidEmailOrPassword, "Invalid Email or Password");
        public static Error UserNotFound = new Error(ErrorCode.UserNotFound, "User Not Found");
    }
}
