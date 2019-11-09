using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.Configuration
{
    public class AppSettings
    {        
        public ReleaseEnvironment Environment { get; set; }
        public string ApplicationName { get; set; }
        public string TokenKey { get; set; }
        public string TokenIssuer { get; set; }
        public string TokenAudience { get; set; }
        public int TokenDurationMinutes { get; set; }
        public string TokenDomain { get; set; }
        public int FederationCodeDurationDays { get; set; }
        public string EmailVerificationUrl { get; set; }
        public string PasswordResetUrl { get; set; }
        public string EmailSenderUrl { get; set; }
        public string EmailSenderUsername { get; set; }
        public string EmailSenderPassword { get; set; }
    }
}
