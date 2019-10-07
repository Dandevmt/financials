using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Api.Authentication
{
    public static class JWTConfiguration
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => 
                {
                    options.TokenValidationParameters = new TokenValidationParameters() 
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "JWT:Issuer",
                        ValidAudience = "JWT:Issuer",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JWT:KEY"))
                    };
                });
        }
    }
}
