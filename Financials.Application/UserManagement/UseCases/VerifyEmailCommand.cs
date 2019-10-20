using Financials.Application.CQRS;
using System;

namespace Financials.Application.UserManagement.UseCases
{
    public class VerifyEmailCommand : ICommand
    {
        public Guid UserId { get; set; }
        public string Code { get; set; }
    }
}
