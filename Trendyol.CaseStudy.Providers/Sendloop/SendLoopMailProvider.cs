using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Trendyol.CaseStudy.Common.Messages;


namespace Trendyol.CaseStudy.Providers.Sendloop
{
    [ContentType(MailContentType.LostPasswordMail, MailContentType.ShipmentMail)]
    public class SendLoopMailProvider : IMailProvider
    {
        private readonly ILogger<SendLoopMailProvider> _logger;

        public SendLoopMailProvider(ILogger<SendLoopMailProvider> logger)
        {
            _logger = logger;
        }

        public Task SendAsync(IMailCommand message)
        {
            _logger.LogInformation(
                $"An email which is content type of {message.ContentType} was sent from {message.From} to {message.To} by using sendloop provider.");

            //throw new IgnoredErrorException("Not Implemented Exception");

            return Task.Factory.StartNew(() =>
            {
                /* do something */
            });
        }
    }
}