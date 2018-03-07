using System;
using System.Collections.Generic;
using GreenPipes;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Trendyol.CaseStudy.Common.Exceptions;
using Trendyol.CaseStudy.Messaging.Configurations;

namespace Trendyol.CaseStudy.Messaging.Configurator
{
    public class BusConfigurator
    {
        private static readonly Lazy<BusConfigurator> _configurator =
            new Lazy<BusConfigurator>(() => new BusConfigurator());

        private readonly List<Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost>> _config =
            new List<Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost>>();

        private Exception[] _exceptions;

        private BusConfigurations _options;


        private BusConfigurator()
        {
        }

        public static BusConfigurator Instance => _configurator.Value;

        public BusConfigurator Options(Action<BusConfigurations> options)
        {
            options.Invoke(_options);

            return this;
        }


        public BusConfigurator Options(BusConfigurations options)
        {
            _options = options;

            return this;
        }


        public BusConfigurator ConfigureRetryOnSpecificExceptions(Exception[] exceptions)
        {
            _exceptions = exceptions;

            return this;
        }

        public BusConfigurator Configure(Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> configurtions)
        {
            _config.Add(configurtions);

            return this;
        }


        public BusConfigurator AddConsumer<TConsumer>(string queueName) where TConsumer : class, IConsumer, new()
        {
            Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> action = (cfg, host) =>
            {
                cfg.ReceiveEndpoint(host, queueName, e => { e.Consumer<TConsumer>(); });
            };

            _config.Add(action);

            return this;
        }

        public BusConfigurator AddConsumer(IConsumer consumer, string queueName)
        {
            Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> action = (cfg, host) =>
            {
                cfg.ReceiveEndpoint(host, queueName, e => { e.Instance(consumer); });
            };

            _config.Add(action);

            return this;
        }


        public IBusControl Build()
        {
            return ConfigureBus(_options);
        }

        private IBusControl ConfigureBus(BusConfigurations options,
            Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> configurtions = null)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(options.HostName), h =>
                {
                    h.Username(options.Username);
                    h.Password(options.Password);
                });

                foreach (var action in _config) action.Invoke(cfg, host);

                ConfigureIncrementalRetryPolicy(cfg);
                ConfigureCircuitBreaker(cfg);
                ConfigureDelayedExchangeMessageScheduler(cfg);
                ConfigureMessageScheduler(cfg);
                UseRateLimiter(cfg);
                UseInMemoryScheduler(cfg);


                configurtions?.Invoke(cfg, host);
            });
        }


        private void ConfigureIncrementalRetryPolicy(IRabbitMqBusFactoryConfigurator cfg)
        {
            if (_options.RetryPolicy.Enabled && _options.RetryPolicy.RetryLimit.HasValue &&
                _options.RetryPolicy.InitialInterval.HasValue && _options.RetryPolicy.IntervalIncrement.HasValue)
                cfg.UseRetry(retryConfig =>
                {
                    if (_exceptions != null)
                        foreach (var exp in _exceptions)
                            retryConfig.Handle(exp.GetType());


                        retryConfig.Handle<HandledErrorException>();
                        retryConfig.Ignore<IgnoredErrorException> ();


                    retryConfig.Incremental(_options.RetryPolicy.RetryLimit.Value,
                        TimeSpan.FromMinutes(_options.RetryPolicy.InitialInterval.Value),
                        TimeSpan.FromMinutes(_options.RetryPolicy.IntervalIncrement.Value));
                });
        }


        private void UseInMemoryScheduler(IRabbitMqBusFactoryConfigurator cfg)
        {
            if (_options.UseInMemoryScheduler) cfg.UseInMemoryScheduler();
        }

        private void ConfigureCircuitBreaker(IRabbitMqBusFactoryConfigurator cfg)
        {
            if (_options.CircuitBreaker.Enabled && _options.CircuitBreaker.TripThreshold.HasValue &&
                _options.CircuitBreaker.ActiveThreshold.HasValue && _options.CircuitBreaker.ResetInterval.HasValue)
                cfg.UseCircuitBreaker(cb =>
                {
                    cb.TripThreshold = _options.CircuitBreaker.TripThreshold.Value;
                    cb.ActiveThreshold = _options.CircuitBreaker.ActiveThreshold.Value;
                    cb.ResetInterval = TimeSpan.FromMinutes(_options.CircuitBreaker.ResetInterval.Value);
                });
        }

        private void ConfigureDelayedExchangeMessageScheduler(IRabbitMqBusFactoryConfigurator cfg)
        {
            if (_options.DelayedMessage.Enabled) cfg.UseDelayedExchangeMessageScheduler();
        }

        private void ConfigureMessageScheduler(IRabbitMqBusFactoryConfigurator cfg)
        {
            if (_options.MessageScheduler.Enabled)
                cfg.UseMessageScheduler(new Uri(string.Concat(_options.HostName, "/",
                    _options.MessageScheduler.QuartzEndpoint)));
        }

        private void UseRateLimiter(IRabbitMqBusFactoryConfigurator cfg)
        {
            if (_options.RateLimit.Enabled && _options.RateLimit.Limit.HasValue &&
                _options.RateLimit.LimiterInterval.HasValue)
                cfg.UseRateLimit(_options.RateLimit.Limit.Value,
                    TimeSpan.FromSeconds(_options.RateLimit.LimiterInterval.Value));
        }
    }
}