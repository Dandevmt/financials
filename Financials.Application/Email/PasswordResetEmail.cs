using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.Email
{
    class PasswordResetEmail : EmailMessage
    {
        public override string TemplateName { get; set; } = "PasswordResetEmail";
        public override string Subject { get; set; } = "OFB Butte: Password Reset";
        public string Url { get; set; }
    }
}
