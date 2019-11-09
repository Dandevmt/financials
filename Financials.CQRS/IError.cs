using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.CQRS
{
    public interface IError
    {
        int Code { get; }
        IError AddError(IError error);
    }
}
