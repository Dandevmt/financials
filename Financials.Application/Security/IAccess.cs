using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.Security
{
    public interface IAccess
    {
        bool CanDo(Permission permission);
    }
}
