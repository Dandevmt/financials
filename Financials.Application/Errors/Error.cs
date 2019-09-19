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

        public void Throw()
        {
            throw new ErrorException(Code, Message, Description);
        }

        public void Throw(Exception innerException)
        {
            throw new ErrorException(Code, Message, Description, innerException);
        }

        public static Error EmailExists = new Error(ErrorCode.Validation, "Email already exists", "Please enter a activation code, login, reset your password or contact the organization");
    }
}
