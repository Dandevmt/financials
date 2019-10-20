using Financials.Application.CQRS;
using System;

namespace Financials.Application.UserManagement.Commands
{
    public class VerifyEmailCommand : ICommand
    {
        public Guid UserId { get; }
        public string Code { get; }

        public VerifyEmailCommand(Guid userId, string code)
        {
            UserId = userId;
            Code = code;
        }
    }
}
