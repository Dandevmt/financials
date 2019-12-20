using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.UserManagement
{
    public interface IHasherService
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string password);
    }
}
