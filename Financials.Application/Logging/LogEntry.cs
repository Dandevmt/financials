using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Financials.Application.Logging
{
    public class LogEntry
    {
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}
