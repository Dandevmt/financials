using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.Security
{
    public interface IPermissionRequired
    {
        Permission PermissionRequired { get; }
    }
}
