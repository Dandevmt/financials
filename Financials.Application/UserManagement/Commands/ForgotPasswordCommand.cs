using Financials.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.UserManagement.Commands
{
    public class ForgotPasswordCommand : ICommand
    {
        public string Email { get; }

        public ForgotPasswordCommand(string email)
        {
            Email = email;
        }
    }
}
