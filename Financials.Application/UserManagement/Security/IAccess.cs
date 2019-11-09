using Financials.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.UserManagement.Security
{
    public interface IAccess
    {
        bool CanDo(string tenantId, string application, string permission);
        User CurrentUser();
    }
}
