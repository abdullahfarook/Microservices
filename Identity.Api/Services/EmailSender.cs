using EventBus.Common.Abstractions;
using Identity.Api.Core;
using Identity.Api.Events;
using Microservices.Core;
using Newtonsoft.Json;

namespace Identity.Api.Services
{
    public class EmailSender:IEmailSender
    {
        private IEventBus _eventBus;

        public EmailSender(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void SendEmail(string recepientName, string recepientEmail, string subject, IEmailArgument arguments, bool isHtml = true)
        {
            _eventBus.Publish(new SendEmailIntegrationEvent
            {
                RecipientName = recepientName,
                RecipientEmail = recepientEmail,
                Subject = subject,
                TemplateUrl = FromLocation(arguments),
                IsHtml = isHtml,
                Arguments = JsonConvert.SerializeObject(arguments)

            });
        }
        private string FromLocation(IEmailArgument email) =>
            $"{Extensions.ApiUrl}/static/templates/email/{email.GetType().Name}.html";
    }
}
