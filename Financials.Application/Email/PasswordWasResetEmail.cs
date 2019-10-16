using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.Email
{
    public class PasswordWasResetEmail : EmailMessage
    {
        public override string TemplateName { get; set; } = "PasswordWasResetEmail";
        public override string Subject { get; set; } = "OFB Butte: Password Reset";
    }
}
