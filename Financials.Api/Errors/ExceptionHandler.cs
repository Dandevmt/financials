using Financials.Application.Errors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Financials.Api.Errors
{
    public static class ExceptionHandler
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
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
                        if (contextFeature.Error is ForbiddenException forbidden)
                        {
                            context.Response.StatusCode = (int)forbidden.Error.Code;
                            await context.Response.WriteAsync(forbidden.Error.ToString());
                        } else
                        {
                            await context.Response.WriteAsync(contextFeature.Error.ToString());
                        }                        
                    }
                });
            });
        }
    }
}
