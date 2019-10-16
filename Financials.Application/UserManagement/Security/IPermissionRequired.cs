using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.UserManagement.Security
{
    public interface IPermissionRequired
    {
        Permission PermissionRequired { get; }
    }
}
