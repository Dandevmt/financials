using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.CQRS
{
    public interface IProvider
    {
        object GetService(Type type);
    }
}
