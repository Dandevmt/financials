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

        public void Throw(string description = null, Exception innerException = null)
        {
            Description = description ?? Description;
            throw new ErrorException(this, innerException);
        }

        public override string ToString()
        {
            return $"{{\"code\":{(int)Code},\"message\":\"{Message}\",\"description\":\"{Description}\"}}";
        }

        public static Error Forbidden() => new Error(ErrorCode.Forbidden, "Permission Denied");

    }
}
