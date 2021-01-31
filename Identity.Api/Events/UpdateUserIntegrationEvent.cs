using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventBus.Common.Events;

namespace Identity.Api.Events
{
    public class UpdateUserIntegrationEvent:IntegrationEvent
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Pic { get; set; }
        public string Email { get; set; }
    }
}
