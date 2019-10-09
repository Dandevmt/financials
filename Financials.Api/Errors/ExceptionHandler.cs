using Financials.Application.Errors;
using Financials.Application.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Financials.Api.Errors
{
    public static class ExceptionHandler
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        if (contextFeature.Error is ErrorException ex)
                        {
                            logger.Log(new LogEntry() { LogLevel = LogLevel.Error, Exception = ex, Message = ex.Error.Message });
                            context.Response.StatusCode = (int)ex.Error.Code;
                            await context.Response.WriteAsync(ex.ToString());
                        }
                        else
                        { 
                            logger.Log(new LogEntry() { LogLevel = LogLevel.Fatal, Exception = contextFeature.Error, Message = contextFeature.Error.Message });
                            await context.Response.WriteAsync(contextFeature.Error.ToString());
                        }                        
                    }
                });
            });
        }
    }
}
