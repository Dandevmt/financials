using System;

namespace Financials.Application.UserManagement.UseCases
{
    public class VerifyEmailInput
    {
        public Guid UserId { get; set; }
        public string Code { get; set; }
    }
}
