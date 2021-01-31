using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventBus.Common.Events;

namespace Payment.Api.Events
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
}
