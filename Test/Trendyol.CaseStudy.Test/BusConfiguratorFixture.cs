using System;
using System.Collections.Generic;
using System.Text;
using MassTransit;
using Trendyol.CaseStudy.Messaging.Configurations;
using Trendyol.CaseStudy.Messaging.Configurator;
using Trendyol.CaseStudy.Test.Bus;

namespace Trendyol.CaseStudy.Test
{
   public class BusConfiguratorFixture : IDisposable
    {
        public readonly IBusControl _busControl;
        public readonly TestMailCommandConsumer _testConsumer;
        public readonly BusConfigurations _busConfigurations;
        public string queueName = "unit_test";

        public BusConfiguratorFixture()
        {
            _testConsumer = new TestMailCommandConsumer();

            _busConfigurations = new BusConfigurations()
            {
                HostName = "rabbitmq://localhost",
                Username = "guest",
                Password = "guest",
                RetryPolicy = new IncrementalRetryPolicyOptions()
                {
                    Enabled = true,
                    InitialInterval = 2,
                    IntervalIncrement = 5,
                    RetryLimit = 5
                }
            };

            var config = BusConfigurator.Instance
                .Options(_busConfigurations).Configure((cfg, host) =>
                {
                    cfg.ReceiveEndpoint(queueName,
                        e =>
                        {
                            e.Consumer(() => _testConsumer);
                            
                           

                        });
                });

            _busControl = config.Build();

            _busControl.Start();
        }

        public void Dispose()
        {
            _busControl?.Stop();
        }
    }
}
