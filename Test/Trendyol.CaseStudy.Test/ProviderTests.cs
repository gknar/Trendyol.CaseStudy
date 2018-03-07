using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Trendyol.CaseStudy.Common.Messages;
using Trendyol.CaseStudy.Providers;
using Trendyol.CaseStudy.Providers.Factories;
using Trendyol.CaseStudy.Providers.Sendgrid;
using Trendyol.CaseStudy.Providers.Sendloop;
using Xunit;

namespace Trendyol.CaseStudy.Test
{
   public class ProviderTests
    {

        public static IContainer Container { get; private set; }
        
        public ProviderTests()
        {
            // dependency injection
            var services = new ServiceCollection();

            services.AddSingleton(
                new LoggerFactory().AddConsole()
            );

            services.AddLogging();

            // autofac
            var builder = new ContainerBuilder();

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


            // build and populate
            builder.Populate(services);

            Container = builder.Build();
        }

        [Fact]
        public void GetProvider_ShouldAssertTrue_WhenMailContentIsOrderMail()
        {
            var providerFactory = Container.Resolve<IMailProviderFactory>();
            
            var actual = providerFactory.GetProvider(MailContentType.OrderMail).GetType();

            var expected = typeof(SendgridMailProvider);

            Assert.Equal(actual, expected);
            
        }

        [Fact]
        public void GetProvider_ShouldAssertTrue_WhenMailContentIsShipmentMail()
        {
            var providerFactory = Container.Resolve<IMailProviderFactory>();

            var actual = providerFactory.GetProvider(MailContentType.ShipmentMail).GetType();

            var expected = typeof(SendLoopMailProvider);

            Assert.Equal(actual, expected);

        }


    }
}
