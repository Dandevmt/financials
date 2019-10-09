using Financials.Application.Configuration;
using Financials.Application.Logging;
using MongoDB.Driver;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Financials.Api.Logging
{
    public class Logger<T> : Application.Logging.ILogger
    {
        private readonly Serilog.ILogger log;

        public Logger(IMongoDatabase db)
        {
            log = new LoggerConfiguration()
                .WriteTo
                .MongoDBCapped(db, Serilog.Events.LogEventLevel.Information)
                .CreateLogger()
                .ForContext(typeof(T));
        }

        public void Log(LogEntry logEntry)
        {
            switch (logEntry.LogLevel)
            {
                case LogLevel.Information:
                    log.Information(logEntry.Exception, logEntry.Message);
                    break;
                case LogLevel.Debug:
                    log.Debug(logEntry.Exception, logEntry.Message);
                    break;
                case LogLevel.Warning:
                    log.Warning(logEntry.Exception, logEntry.Message);
                    break;
                case LogLevel.Error:
                    log.Error(logEntry.Exception, logEntry.Message);
                    break;
                case LogLevel.Fatal:
                    log.Fatal(logEntry.Exception, logEntry.Message);
                    break;
                default:
                    break;
            }
        }
    }
}
