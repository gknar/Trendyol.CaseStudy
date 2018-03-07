using System.Threading.Tasks;

namespace Trendyol.CaseStudy.Messaging
{
    public interface IReceiver
    {
        Task StartAsync();
        Task StopAsync();
    }
}