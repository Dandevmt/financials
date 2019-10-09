using Financials.Application.Configuration;
using Financials.Application.Logging;
using Financials.Application.Security;
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
        private readonly Func<IAccess> accessFactory;

        public Logger(IMongoDatabase db, Func<IAccess> accessFactory)
        {
            this.accessFactory = accessFactory;
            log = new LoggerConfiguration()
                .WriteTo
                .MongoDBCapped(db, Serilog.Events.LogEventLevel.Information)
                .CreateLogger()
                .ForContext(typeof(T));
        }

        public void Log(LogEntry logEntry)
        {
            var user = accessFactory().CurrentUser();
            switch (logEntry.LogLevel)
            {
                case LogLevel.Information:
                    log.ForContext("userId", user?.Id).Information(logEntry.Exception, logEntry.Message);
                    break;
                case LogLevel.Debug:
                    log.ForContext("userId", user?.Id).Debug(logEntry.Exception, logEntry.Message);
                    break;
                case LogLevel.Warning:
                    log.ForContext("userId", user?.Id).Warning(logEntry.Exception, logEntry.Message);
                    break;
                case LogLevel.Error:
                    log.ForContext("userId", user?.Id).Error(logEntry.Exception, logEntry.Message);
                    break;
                case LogLevel.Fatal:
                    log.ForContext("userId", user?.Id).Fatal(logEntry.Exception, logEntry.Message);
                    break;
                default:
                    break;
            }
        }
    }
}
