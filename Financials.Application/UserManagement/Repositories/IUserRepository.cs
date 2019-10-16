using Financials.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.UserManagement.Repositories
{
    public interface IUserRepository
    {
        User Get(Guid id);
        IEnumerable<User> Get(int pageSize, int pageNumber, string sortField);
        IEnumerable<User> GetAll();
        User Add(User user);
        User Update(User user);
        User Delete(Guid id);
    }
}
