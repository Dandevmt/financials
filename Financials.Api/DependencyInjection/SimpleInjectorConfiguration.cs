using Financials.Api.Authentication;
using Financials.Api.Logging;
using Financials.Application;
using Financials.Application.Configuration;
using Financials.Application.Email;
using Financials.Application.Logging;
using Financials.Application.UserManagement.Security;
using Financials.Application.UserManagement;
using Financials.Database;
using Financials.Infrastructure.Codes;
using Financials.Infrastructure.Email;
using Financials.Infrastructure.Hashing;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financials.CQRS;
using Financials.UserManagement;

namespace Financials.Api.DependencyInjection
{
    public static class SimpleInjectorConfiguration
    {
        public static void Setup(IServiceCollection services, Container container)
        {
            services.AddSimpleInjector(container, options =>
            {
                options.AddAspNetCore()
                .AddControllerActivation()
                .AddViewComponentActivation();
            });
        }

        public static void ConfigureApp(IApplicationBuilder app, IConfiguration configuration, Container container)
        {
            foreach (var pair in configuration.GetChildren().OrderBy(p => p.Path))
            {
                Console.WriteLine($"{pair.Path} - {pair.Value}");
            }
            app.UseSimpleInjector(container);

            // Connection Strings
            var connectionStrings = configuration.GetSection(nameof(ConnectionStrings)).Get<ConnectionStrings>();
            container.RegisterInstance(connectionStrings);

            // App Settings
            var appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
            container.RegisterInstance(appSettings);

            // Configure Financials Database
            container.RegisterSingleton(() => 
            {
                var client = new MongoClient(connectionStrings.Financials.DataSource);
                return client.GetDatabase(connectionStrings.Financials.Database);
            });

            // Other Singletons
            container.RegisterSingleton<ICodeGenerator, CodeGenerator>();
            container.RegisterSingleton<IHasherService>(() => new PasswordHasher());
            container.RegisterSingleton<IEmailSender>(() => new EmailSender(new MailKit.Net.Smtp.SmtpClient(), appSettings.EmailSenderUrl, appSettings.EmailSenderUsername, appSettings.EmailSenderPassword, appSettings.Environment != ReleaseEnvironment.Prod));

            // Logging
            container.RegisterConditional(
                typeof(ILogger),
                c => typeof(Logger<>).MakeGenericType(c.Consumer?.ImplementationType ?? typeof(Program)),
                Lifestyle.Singleton,
                c => true);

            // Repositories
            container.Register<Application.UserManagement.Repositories.ITenantRepository, TenantRepository>(Lifestyle.Scoped);
            container.Register<IUserRepository, UserRepository>(Lifestyle.Scoped);
            container.Register<IClientSessionHandle>(() =>
            {
                return container.GetInstance<IMongoDatabase>().Client.StartSession();
            }, Lifestyle.Scoped);

            // Access
            container.RegisterInstance<Func<IAccess>>(() => container.GetInstance<IAccess>());
            container.Register<ITokenBuilder, TokenBuilder>(Lifestyle.Singleton);
            //container.Register<IAccess, Access>(Lifestyle.Scoped);

            // Command Dispatcher and Container Wrapper
            container.RegisterSingleton<IProvider, ProviderWrapper>();
            container.RegisterSingleton<Dispatcher>();

            // Commands
            container.Register(typeof(ICommandHandler<,>), AppDomain.CurrentDomain.GetAssemblies());

            // Queries
            container.Register(typeof(IQueryHandler<,>), AppDomain.CurrentDomain.GetAssemblies());

            // Events
           container.Register(typeof(IEventHandler<>), AppDomain.CurrentDomain.GetAssemblies());

            // Decorators
            //container.RegisterDecorator(typeof(ICommandHandler<,>), typeof(RequirePermissionDecorator<>));
            //container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(RequirePermissionQueryDecorator<,>));
            //container.RegisterDecorator(typeof(ICommandHandler<,>), typeof(UnitOfWorkDecorator<>),
            //    context => context.ImplementationType.GetCustomAttributes(typeof(UnitOfWorkDecoratorAttribute), false).Any());
            

            container.Verify();
        }
    }
}
