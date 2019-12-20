using Financials.Common.Email;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.Email
{
    public class PasswordWasResetEmail : EmailMessage
    {
        public override string Template { get; set; } = "PasswordWasResetEmail";
        public override string Subject { get; set; } = "OFB Butte: Password Reset";
    }
}
