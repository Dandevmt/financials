using Financials.Common.Email;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.Email
{
    public class VerifyEmailEmail : EmailMessage
    {
        public override string Template { get; set; } = "VerifyEmail";
        public override string Subject { get; set; } = "OFB Butte: Password Verification";
        public string Url { get; set; }
    }
}
