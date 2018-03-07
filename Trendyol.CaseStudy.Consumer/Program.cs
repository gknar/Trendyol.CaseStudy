using System;
using System.IO;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Trendyol.CaseStudy.Consumer.Handlers;
using Trendyol.CaseStudy.Messaging;
using Trendyol.CaseStudy.Messaging.Configurations;
using Trendyol.CaseStudy.Providers;
using Trendyol.CaseStudy.Providers.Factories;

namespace Trendyol.CaseStudy.Consumer
{
    internal class Program
    {
        private static IConfigurationRoot Configuration;
        public static IContainer Container { get; private set; }

        private static void Main(string[] args)
        {
            // configuration
            var cgfbuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = cgfbuilder.Build();

            // dependency injection
            var services = new ServiceCollection();

            services.AddSingleton(
                new LoggerFactory().AddConsole()
            );

            services.AddLogging();

            // autofac
            var builder = new ContainerBuilder();


            builder.RegisterType<Startup>().SingleInstance();
            builder.RegisterType<Receiver>().As<IReceiver>().SingleInstance();
            builder.RegisterInstance(GetConfiguration()).SingleInstance();
            builder.RegisterType<MailCommandConsumer>().InstancePerLifetimeScope();

            // mail provider registration
            RegisterMailProviders(builder);

            builder.Populate(services);

            Container = builder.Build();

            // provider
            var provider = new AutofacServiceProvider(Container);

            // run
            provider.GetService<Startup>().Run();

            Console.ReadKey();
        }


        private static void RegisterMailProviders(ContainerBuilder builder)
        {
            // factory
            builder.RegisterType<MailProviderFactory>().As<IMailProviderFactory>().SingleInstance();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToArray();

            // scan provider
            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => t.Name.EndsWith("Provider"))
                .As<IMailProvider>()
                .Named<IMailProvider>(t => t.Name)
                .AsImplementedInterfaces()
                .SingleInstance();
        }

        private static BusConfigurations GetConfiguration()
        {
            var configurations = new BusConfigurations();
            Configuration.GetSection("BusConfigurations").Bind(configurations);
            return configurations;
        }
    }
}