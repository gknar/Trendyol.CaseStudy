using System.Threading.Tasks;

namespace Trendyol.CaseStudy.Messaging
{
    public interface IPublisher
    {
        Task PublishAsync<TMessage>(TMessage message);
        Task SendAsync<TMessage>(TMessage message, string queueName);
    }
}