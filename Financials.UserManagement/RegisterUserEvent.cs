using Financials.CQRS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.UserManagement
{
    public class RegisterUserEvent : IEvent
    {
        public string Email { get; }
        public string Username { get; }
        public string VerificationCode { get; }

        public RegisterUserEvent(string email, string username, string verificationCode)
        {
            Email = email;
            Username = username;
            VerificationCode = verificationCode;
        }
    }

    public class RegisterUserEventHandler : IEventHandler<RegisterUserEvent>
    {
        private IEmailSender emailSender;

        public RegisterUserEventHandler(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        public async Task Handle(RegisterUserEvent evnt)
        {
            await emailSender.Send(new VerifyEmailMessage(evnt.Username, evnt.VerificationCode) { To = evnt.Email });
        }
    }
}
