using System.Collections.Generic;

namespace Trendyol.CaseStudy.Messaging.Configurations
{
    public class BusConfigurations
    {
        public BusConfigurations()
        {
            ConsumeOnSpecifiedQueue = new List<string>();
            CircuitBreaker = new CircuitBreakerOptions();
            RetryPolicy = new IncrementalRetryPolicyOptions();
            DelayedMessage = new DelayedExchangeMessageSchedulerOptions();
            MessageScheduler = new MessageSchedulerOptions();
            RateLimit = new RateLimitOptions();

            RequestTimeout = 20;
        }

        public string HostName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RequestTimeout { get; set; }


        public bool UseInMemoryScheduler { get; set; }


        public List<string> ConsumeOnSpecifiedQueue { get; set; }

        public CircuitBreakerOptions CircuitBreaker { get; set; }
        public IncrementalRetryPolicyOptions RetryPolicy { get; set; }
        public DelayedExchangeMessageSchedulerOptions DelayedMessage { get; set; }
        public MessageSchedulerOptions MessageScheduler { get; set; }

        public RateLimitOptions RateLimit { get; set; }
    }


    public class CircuitBreakerOptions
    {
        public bool Enabled { get; set; }
        public int? TripThreshold { get; set; }
        public int? ActiveThreshold { get; set; }
        public int? ResetInterval { get; set; }
    }

    public class IncrementalRetryPolicyOptions
    {
        public bool Enabled { get; set; }
        public int? RetryLimit { get; set; }
        public int? InitialInterval { get; set; }
        public int? IntervalIncrement { get; set; }
    }

    public class DelayedExchangeMessageSchedulerOptions
    {
        public bool Enabled { get; set; }
    }

    public class MessageSchedulerOptions
    {
        public bool Enabled { get; set; }

        public string QuartzEndpoint { get; set; }
    }


    public class RateLimitOptions
    {
        public bool Enabled { get; set; }

        public int? Limit { get; set; }

        public int? LimiterInterval { get; set; }
    }
}