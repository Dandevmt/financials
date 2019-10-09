using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.Errors
{
    public class ForbiddenException : ErrorException
    {
        public ForbiddenException(Error error) : base(error)
        {
        }

        public ForbiddenException(Error error, Exception innerException) : base(error, innerException)
        {
        }
    }
}
