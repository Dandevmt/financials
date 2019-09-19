using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.Errors
{
    public class ErrorException : Exception
    {
        public ErrorCode Code { get; set; }
        public string Description { get; set; }
        
        public ErrorException(ErrorCode code, string message) : base(message)
        {
            Code = code;
        }

        public ErrorException(ErrorCode code, string message, string description) : this(code, message)
        {
            Description = description;
        }

        public ErrorException(ErrorCode code, string message, Exception innerException) : base(message, innerException)
        {
            Code = code;
        }

        public ErrorException(ErrorCode code, string message, string description, Exception innerException) : this(code, message, innerException)
        {
            Description = description;
        }

        public override string ToString()
        {
            return $"Code: {Code}{Environment.NewLine}Message: {Message}{Environment.NewLine}Description: {Description}{Environment.NewLine}Inner Exception: {InnerException}";
        }
    }
}
