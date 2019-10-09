using System;

namespace Financials.Application.Users.UseCases
{
    public class VerifyEmailInput
    {
        public Guid UserId { get; set; }
        public string Code { get; set; }
    }
}
