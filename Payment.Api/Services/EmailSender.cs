using System;
using System.Threading.Tasks;
using EventBus.Common.Abstractions;
using EventBus.Common.Extensions;
using Microservices.Core;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using Payment.Api.Core;
using Payment.Api.Events;

namespace Payment.Api.Services
{
    public class EmailSender : IEmailSender
    {
        private IEventBus _eventBus;

        public EmailSender(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void SendEmail(string recepientName, string recepientEmail, string subject, IEmailArgument argument, bool isHtml = true)
        {
            _eventBus.Publish(new SendEmailIntegrationEvent
            {
                RecipientName = recepientName,
                RecipientEmail = recepientEmail,
                Subject = subject,
                TemplateUrl = FromLocation(argument),
                IsHtml = isHtml,
                Arguments = argument

            });
        }
        private string FromLocation(IEmailArgument email) =>
            $"{Extensions.ApiUrl}/static/templates/email/{email.GetType().Name}.html";
    }
}
