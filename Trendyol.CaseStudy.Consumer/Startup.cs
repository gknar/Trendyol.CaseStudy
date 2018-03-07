using Microsoft.Extensions.Logging;
using Trendyol.CaseStudy.Messaging;
using Trendyol.CaseStudy.Messaging.Configurations;

namespace Trendyol.CaseStudy.Consumer
{
    public class Startup
    {
        private readonly BusConfigurations _busConfigurations;
        private readonly ILogger<Startup> _logger;

        private readonly IReceiver _receiver;

        public Startup(IReceiver receiver, BusConfigurations busConfigurations, ILogger<Startup> logger)
        {
            _receiver = receiver;
            _busConfigurations = busConfigurations;
            _logger = logger;
        }


        public void Run()
        {
            _logger.LogInformation($"Consumer app has been started");
            _receiver.StartAsync();
        }

        public void Stop()
        {
            _logger.LogInformation($"Consumer app has been stopped");

            _receiver.StartAsync();
        }
    }
}