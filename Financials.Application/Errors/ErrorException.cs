using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.Errors
{
    public class ErrorException : Exception
    {
        public Error Error { get; private set; }
        
        public ErrorException(Error error) : base(error.Message)
        {
            Error = error;
        }

        public ErrorException(Error error, Exception innerException) : base(error.Message, innerException)
        {
            Error = error;
        }

        public override string ToString()
        {
            return $"Code: {Error.Code}{Environment.NewLine}Message: {Error.Message}{Environment.NewLine}Description: {Error.Description}{Environment.NewLine}Inner Exception: {InnerException}";
        }
    }
}
