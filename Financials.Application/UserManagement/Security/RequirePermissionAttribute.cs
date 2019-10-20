using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.UserManagement.Security
{
    public class RequirePermissionAttribute : Attribute
    {
        public Permission Permission { get; }
        public RequirePermissionAttribute(Permission permission)
        {
            Permission = permission;
        }
    }
}
