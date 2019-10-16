using Financials.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.UserManagement.Repositories
{
    public interface ICredentialRepository
    {
        Credentials Get(Guid userId);
        Credentials Get(string email);
        Credentials Add(Credentials credentials);
        Credentials UpdateOne(Credentials credentials);
    }
}
