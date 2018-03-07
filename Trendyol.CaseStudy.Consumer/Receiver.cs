using System;
using System.Threading.Tasks;
using Autofac;
using MassTransit;
using Trendyol.CaseStudy.Consumer.Handlers;
using Trendyol.CaseStudy.Messaging;
using Trendyol.CaseStudy.Messaging.Configurations;
using Trendyol.CaseStudy.Messaging.Configurator;

namespace Trendyol.CaseStudy.Consumer
{
    public class Receiver : IReceiver, IDisposable
    {
        private readonly IBusControl _bus;
        private bool _started;

        public Receiver(BusConfigurations options, ILifetimeScope scope)
        {
            var config = BusConfigurator.Instance
                .Options(options)
                .Configure((cfg, host) =>
                {
                    foreach (var queue in options.ConsumeOnSpecifiedQueue)
                        cfg.ReceiveEndpoint(queue,
                            e => { e.Consumer(typeof(MailCommandConsumer), type => scope.Resolve(type)); });
                });

            _bus = config.Build();
        }


        public void Dispose()
        {
            if (_started)
                _bus.Stop();
        }


        public async Task StartAsync()
        {
            if (_started) throw new InvalidOperationException("Bus is already started");

            await _bus.StartAsync();

            _started = true;
        }

        public async Task StopAsync()
        {
            if (!_started) throw new InvalidOperationException("Bus is already stoped");

            await _bus.StopAsync();

            _started = false;
        }


        private async Task EnsureBus()
        {
            if (!_started)
            {
                await _bus.StartAsync();
                _started = true;
            }
        }
    }
}