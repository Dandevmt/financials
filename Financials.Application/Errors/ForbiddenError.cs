using Financials.Application.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.Errors
{
    public class ForbiddenError : Error
    {
        protected ForbiddenError(ErrorCode code, string message) : base(code, message)
        {
        }

        protected ForbiddenError(ErrorCode code, string message, string description) : base(code, message, description)
        {
        }

        public static void Throw(Permission permission)
        {
            throw new ForbiddenException(new ForbiddenError(ErrorCode.Forbidden, "Permission denied for " + permission.ToString()));
        }
    }
}
