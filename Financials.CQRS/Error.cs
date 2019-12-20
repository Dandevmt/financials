using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.CQRS
{
    public class Error : IError
    {
        public int Code { get; protected set; }

        public string Message { get; protected set; }

        public Error(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
