using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EventBus.Common.Abstractions;
using EventBus.Common.Events;
using Mail.Api.Core;
using Microservices.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using IEmailSender = Mail.Api.Services.IEmailSender;

namespace Mail.Api.Events
{
    public class SendEmailIntegrationEvent : IntegrationEvent
    {
        public string Type { get; set; }
        public string TemplateUrl { get; set; }
        public string RecipientName { get; set; }

        public string RecipientEmail { get; set; }

        public string Subject { get; set; }
        public bool IsHtml { get; set; } = true;

        public dynamic Arguments { get; set; }
    }
    public class SendEmailIntegrationEventHandler : IIntegrationEventHandler<SendEmailIntegrationEvent>
    {
        private IEmailSender _emailSender;
        private ILogger _logger;
        private IRemoteFileService _httpClient;

        public SendEmailIntegrationEventHandler(IRemoteFileService httpClient, IEmailSender emailSender, ILogger<SendEmailIntegrationEventHandler> logger)
        {
            _httpClient = httpClient;
            _emailSender = emailSender;
            _logger = logger;
        }
        public async Task Handle(SendEmailIntegrationEvent @event)
        {
            _logger.LogInformation("Email Sent Event");
            var arguments = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(@event.Arguments);
            var stream = await _httpClient
                .GetRemoteFile(@event.TemplateUrl);

            var template = stream.ReadToEnd();
            foreach (var key in arguments.Keys)
            {
                // check if the value is not null or empty.
                if (!string.IsNullOrEmpty(arguments[key]))
                {
                   template= template.Replace($"[{key}]", arguments[key]);
                }
            }
            var res = await _emailSender.SendEmailAsync(@event.RecipientName, @event.RecipientEmail, @event.Subject, template);
            if (res.Succeeded)
            {
                _logger.LogInformation($"Email of type {@event.Type} sent to {@event.RecipientEmail}");
            }
            else
            {
                throw res.Exception;
            }
        }
    }
}
