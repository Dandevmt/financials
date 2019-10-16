using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financials.Api.Errors;
using Financials.Application.UserManagement.Codes;
using Financials.Application.Configuration;
using Financials.Application.UserManagement;
using Financials.Infrastructure.Codes;
using Financials.Infrastructure.Hashing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SimpleInjector;

namespace Financials.Api
{
    public class Startup
    {
        private Container container = new Container();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
            Authentication.JWTConfiguration.Configure(services, appSettings);

            services.AddControllers();
            services.AddLogging();

            DependencyInjection.SimpleInjectorConfiguration.Setup(services, container);  
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            DependencyInjection.SimpleInjectorConfiguration.ConfigureApp(app, Configuration, container);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureExceptionHandler(container.GetInstance<Application.Logging.ILogger>());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
