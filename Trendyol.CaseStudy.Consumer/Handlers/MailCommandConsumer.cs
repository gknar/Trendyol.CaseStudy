using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Trendyol.CaseStudy.Common.Messages;
using Trendyol.CaseStudy.Providers;

namespace Trendyol.CaseStudy.Consumer.Handlers
{
    public class MailCommandConsumer : IConsumer<IMailCommand>
    {
        private readonly ILogger<MailCommandConsumer> _logger;

        private readonly IMailProviderFactory _mailProviderFactory;

        public MailCommandConsumer(IMailProviderFactory mailProviderFactory, ILogger<MailCommandConsumer> logger)
        {
            _mailProviderFactory = mailProviderFactory;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IMailCommand> context)
        {
            if (context.Message.ScheduleAt.HasValue)
            {
                _logger.LogInformation(
                    $"An email was rescheduled from {context.Message.From} to {context.Message.To} at {context.Message.ScheduleAt.Value.ToShortDateString()}");

                await context.ScheduleSend(context.Message.ScheduleAt.Value, context.Message);
            }
            else
            {
                var provider = _mailProviderFactory.GetProvider(context.Message.ContentType);

                await provider.SendAsync(context.Message);
            }

            await context.CompleteTask;
        }
    }
}