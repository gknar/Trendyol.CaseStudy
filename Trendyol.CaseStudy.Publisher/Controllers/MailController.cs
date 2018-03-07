using System.Net;
using Microsoft.AspNetCore.Mvc;
using Trendyol.CaseStudy.Common.Messages;
using Trendyol.CaseStudy.Messaging;
using Trendyol.CaseStudy.Publisher.Filters;

namespace Trendyol.CaseStudy.Publisher.Controllers
{
    [Produces("application/json")]
    [Route("api/mail")]
    public class MailController : Controller
    {
        private readonly IPublisher _publisher;


        public MailController(IPublisher publisher)
        {
            _publisher = publisher;
        }


        /// <summary>
        /// </summary>
        /// <remarks>
        ///     POST /api/mail
        ///     {
        ///     "From": "from@gmail.com",
        ///     "To": "to@gmail.com",
        ///     "Subject": "Sipariş kargoya verildi",
        ///     "ContentType" :"ShipmentMail",
        ///     "ScheduleAt": "20/03/2018 00:00:00",
        ///     "Body": "
        ///     <p>
        ///         Lorem ipsum dolor sit amet, vivendo sententiae eam te, quo no debitis tibique hendrerit, per sonet vitae eu. An
        ///         choro doming dolorum vel, eam nisl denique et. Est quis consetetur ne. An est commune pertinax, legendos
        ///         antiopam an nam. Quo no eleifend salutatus dissentiet, eam ne reformidans dissentiunt, te atqui salutandi eos.
        ///         Ne oratio inermis quaerendum mel.
        ///     </p>
        ///     "
        ///     }
        /// </remarks>
        /// <param name="mailContent"></param>
        /// <response code="422">If the model is in valid</response>
        /// <response code="200">Returns if it's success</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [ValidateModel]
        public IActionResult Post([FromBody] MailContent mailContent)
        {
            _publisher.SendAsync<IMailCommand>(mailContent, mailContent.ContentType.ToString().ToLower()).Wait();

            return Ok(new
                {
                    Success = true,
                    Code = HttpStatusCode.OK,
                    Message = "Mail message was queued up"
                }
            );
        }
    }
}