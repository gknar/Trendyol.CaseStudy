using Trendyol.CaseStudy.Common.Messages;

namespace Trendyol.CaseStudy.Providers
{
    /// <summary>
    ///     Creates instances of MailProvider.
    /// </summary>
    public interface IMailProviderFactory
    {
        /// <summary>
        ///     Returns the instance of a MailProvider using the given content type.
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        IMailProvider GetProvider(MailContentType contentType);

        /// <summary>
        ///     Returns the instance of a MailProvider using the given MailContent.
        /// </summary>
        /// <param name="mailContent"></param>
        /// <returns></returns>
        IMailProvider GetProvider(MailContent mailContent);
    }
}