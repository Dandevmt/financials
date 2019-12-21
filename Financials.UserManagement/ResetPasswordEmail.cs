using Financials.Common.Email;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.UserManagement
{
    class ResetPasswordEmail : EmailMessage
    {
        public override string Template { get; set; } = @"
            <p>Please reset your password by clicking the link below.</p>
            <p><a href='VerifyUrl'>{{Url}}</a></p>
        ";

        public override string Subject { get; set; } = "Verify Email";

        public string Url { get; set; }

        public ResetPasswordEmail(string user, string code)
        {
            Url = $"https://localhost:4200?code={code}&user={user}";
        }
    }
}
