using System;

namespace Trendyol.CaseStudy.Common.Messages
{
    public interface IMailCommand
    {
        string From { get; set; }
        string To { get; set; }
        string Subject { get; set; }
        string Body { get; set; }
        DateTime? ScheduleAt { get; }
        MailContentType ContentType { get; set; }
    }
}