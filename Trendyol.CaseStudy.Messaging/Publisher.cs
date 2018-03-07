using System;
using System.Threading.Tasks;
using MassTransit;
using Trendyol.CaseStudy.Messaging.Configurations;
using Trendyol.CaseStudy.Messaging.Configurator;

namespace Trendyol.CaseStudy.Messaging
{
    public class Publisher : IPublisher, IDisposable
    {
        private readonly IBusControl _bus;
        private readonly BusConfigurations _options;
        private bool _started;


        public Publisher(BusConfigurations options)
        {
            _options = options;

            _bus = BusConfigurator.Instance.Options(_options).Build();
        }

        public void Dispose()
        {
            if (_started)
                _bus.Stop();
        }

        public async Task PublishAsync<TMessage>(TMessage message)
        {
            await EnsureBus();
            await _bus.Publish((object) message);
            ;
        }

        public async Task SendAsync<TMessage>(TMessage message, string queueName)
        {
            var endpoint = await GetSendEndpoint(queueName);

            await endpoint.Send((object) message);
            ;
        }

        private async Task EnsureBus()
        {
            if (!_started)
            {
                await _bus.StartAsync();
                _started = true;
            }
        }

        private async Task<ISendEndpoint> GetSendEndpoint(string queueName)
        {
            var endpoint = new Uri($"{_options.HostName}/{queueName}");

            return await _bus.GetSendEndpoint(endpoint);
        }
    }
}