using Financials.Api.Authentication;
using Financials.Api.Logging;
using Financials.Application;
using Financials.Application.UserManagement.Codes;
using Financials.Application.Configuration;
using Financials.Application.Email;
using Financials.Application.Logging;
using Financials.Application.UserManagement.Repositories;
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
using Financials.Application.CQRS;

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
            container.RegisterSingleton<IPasswordHasher>(() => new PasswordHasher());
            container.RegisterSingleton<IEmailSender>(() => new EmailSender(new MailKit.Net.Smtp.SmtpClient(), appSettings.EmailSenderUrl, appSettings.EmailSenderUsername, appSettings.EmailSenderPassword, appSettings.Environment != ReleaseEnvironment.Prod));

            // Logging
            container.RegisterConditional(
                typeof(ILogger),
                c => typeof(Logger<>).MakeGenericType(c.Consumer?.ImplementationType ?? typeof(Program)),
                Lifestyle.Singleton,
                c => true);

            // Repositories
            container.Register<IUserRepository, UserRepository>(Lifestyle.Scoped);
            container.Register<IValidationCodeRepository, ValidationCodeRepository>(Lifestyle.Scoped);
            container.Register<ICredentialRepository, CredentialRepository>(Lifestyle.Scoped);
            container.Register<IClientSessionHandle>(() =>
            {
                return container.GetInstance<IMongoDatabase>().Client.StartSession();
            }, Lifestyle.Scoped);

            // Access
            container.RegisterInstance<Func<IAccess>>(() => container.GetInstance<IAccess>());
            container.Register<ITokenBuilder, TokenBuilder>(Lifestyle.Singleton);
            container.Register<IAccess, Access>(Lifestyle.Scoped);

            // Use Case for AOP
            container.Register(typeof(ICommandHandler<>), typeof(ICommandHandler<>).Assembly);
            container.Register(typeof(IUseCase<,>), typeof(IUseCase<,>).Assembly);

            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(RequirePermissionDecorator<>), 
                context => context.ImplementationType.GetCustomAttributes(typeof(RequirePermissionAttribute), false).Any());

            container.RegisterSingleton<IProvider, ProviderWrapper>();
            container.RegisterSingleton<Dispatcher>();

            // Decorators
            container.RegisterDecorator(typeof(IUseCase<,>), typeof(UseCaseUnitOfWorkDecorator<,>));
            container.RegisterDecorator(typeof(IUseCase<,>), typeof(UseCasePermissionDecorator<,>), context => typeof(IPermissionRequired).IsAssignableFrom(context.ImplementationType));



            container.Verify();
        }
    }
}
