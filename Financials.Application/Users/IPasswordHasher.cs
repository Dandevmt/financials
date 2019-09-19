using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.Users
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string password);
    }
}
