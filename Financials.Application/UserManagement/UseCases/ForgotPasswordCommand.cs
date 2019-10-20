using Financials.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.UserManagement.UseCases
{
    public class ForgotPasswordCommand : ICommand
    {
        public string Email { get; set; }
    }
}
