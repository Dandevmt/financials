using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.UserManagement.Security
{
    public interface IRequirePermission
    {
        string TenantId { get; }
        Permission Permission { get; }
    }
}
