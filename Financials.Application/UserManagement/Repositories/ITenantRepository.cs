using Financials.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.UserManagement.Repositories
{
    public interface ITenantRepository
    {
        Tenant Add(Tenant tenant);
        Tenant Update(Tenant tenant);
        Tenant Get(string id);
        IList<Tenant> GetAll();
    }
}
