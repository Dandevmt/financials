using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.Configuration
{
    public class AppSettings
    {
        public ReleaseEnvironment Environment { get; set; }
        public string TokenKey { get; set; }
        public string TokenIssuer { get; set; }
        public string TokenAudience { get; set; }
    }
}
