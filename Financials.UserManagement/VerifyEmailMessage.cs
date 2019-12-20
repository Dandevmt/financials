using Financials.Common.Email;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.UserManagement
{
    public class VerifyEmailMessage : EmailMessage
    {
        public override string Template { get; set; } = @"
            <p>Please verify your email by clicking the link below.</p>
            <p><a href='VerifyUrl'>{{VerifyUrl}}</a></p>
        ";

        public string VerifyUrl { get; set; }

        public VerifyEmailMessage(string user, string code)
        {
            VerifyUrl = $"https://localhost:4200?code={code}&user={user}";
        }
    }
}
