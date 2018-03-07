using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Trendyol.CaseStudy.Common.Messages;

namespace Trendyol.CaseStudy.Providers.Sendgrid
{
    [ContentType(MailContentType.OrderMail)]
    public class SendgridMailProvider : IMailProvider
    {
        private readonly ILogger<SendgridMailProvider> _logger;

        public SendgridMailProvider(ILogger<SendgridMailProvider> logger)
        {
            _logger = logger;
        }

        public  Task SendAsync(IMailCommand message)
        {
            _logger.LogInformation(
                $"An email which is content type of {message.ContentType} was sent from {message.From} to {message.To} by using sendgrid provider.");

            //throw new IgnoredErrorException("Not Implemented Exception");

            return Task.Factory.StartNew(() =>
            {
                /* do something */
            });
        }
    }
}