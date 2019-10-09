using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.Logging
{
    public interface ILogger
    {
        void Log(LogEntry logEntry);
    }
}
