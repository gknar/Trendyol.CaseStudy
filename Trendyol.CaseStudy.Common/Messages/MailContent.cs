using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Trendyol.CaseStudy.Common.Messages
{

    /// <summary>
    /// This class does represent mail message model
    /// </summary>
    public class MailContent : IMailCommand, IValidatableObject
    {


        /// <summary>
        /// Returns the instance of a MailContent
        /// </summary>
        public MailContent()
        {
        }


        /// <summary>
        /// Returns the instance of a MailContent using the given parameters 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="contentType"></param>
        public MailContent(string from, string to, string subject, string body,
            MailContentType contentType)
        {
            From = from;
            To = to;
            Subject = subject;
            Body = body;
            ContentType = contentType;
        }

        /// <summary>
        /// Returns the instance of a MailContent using the given parameters
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="contentType"></param>
        /// <param name="scheduleAt"></param>
        public MailContent(string from, string to, string subject, string body,
            MailContentType contentType, DateTime? scheduleAt)
        {
            From = from;
            To = to;
            Subject = subject;
            Body = body;
            ContentType = contentType;
            ScheduleAt = scheduleAt;
        }

        [Required(ErrorMessage = "The from is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "From", Description = "Sender email address")]
        public string From { get; set; }


        [Required(ErrorMessage = "The to is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "To", Description = "Recipient email address")]
        public string To { get; set; }


        [Required(ErrorMessage = "The subject is required")]
        [Display(Name = "Subject", Description = "Subject of the email")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "The body is required")]
        [Display(Name = "Body",
            Description = "Body of the email. This parameter is optional if you have defined a HtmlBody parameter.")]
        public string Body { get; set; }


        [Required(ErrorMessage = "The content type is required")]
        [Display(Name = "ContentType", Description = "Mail content type of the email.")]
        public MailContentType ContentType { get; set; }


        /// <summary>
        /// Date / Time to schedule email Delivery.
        /// </summary>
        [Display(Name = "Schedule At",
            Description = "The time at which the email is to sent, format : dd/MM/yyyy HH:mm:ss UTC")]
        [DataType(DataType.DateTime)]
        public DateTime? ScheduleAt { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ScheduleAt.HasValue && ScheduleAt.Value < DateTime.UtcNow)
                yield return new ValidationResult(
                    $"ScheduleAt time cannot be smaller  than {DateTime.UtcNow.ToShortDateString()}");
        }

    }
}