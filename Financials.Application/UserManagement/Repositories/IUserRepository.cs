using Financials.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.UserManagement.Repositories
{
    public interface IUserRepository
    {
        User Get(Guid id, string tenantId);
        User GetWithAllTenants(Guid id);
        User Get(string email, string tenantId);
        IEnumerable<User> GetAll(string tenantId);
        User Add(User user);
        User Update(User user);
        User Delete(Guid id);
    }
}
