using Financials.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.UserManagement.Repositories
{
    public interface IUserRepository
    {
        User Get(Guid id);
        User Get(string email);
        IEnumerable<User> Get(string tenantId, int pageSize, int pageNumber, string sortField);
        IEnumerable<User> GetAll(string tenantId);
        User Add(User user);
        User Update(User user);
        User Delete(Guid id);
    }
}
