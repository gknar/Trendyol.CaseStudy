using System.Threading.Tasks;
using Trendyol.CaseStudy.Common.Messages;

namespace Trendyol.CaseStudy.Providers
{
    public interface IMailProvider
    {
        /// <summary>
        /// Send an email
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendAsync(IMailCommand message);
    }
}