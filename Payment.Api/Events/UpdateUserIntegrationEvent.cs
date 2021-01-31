using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EventBus.Common.Abstractions;
using EventBus.Common.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Payment.Api.DataAccess;
using Payment.Api.DataAccess.Model;

namespace Payment.Api.Events
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
    public class UpdateUserIntegrationEventHandler : IIntegrationEventHandler<UpdateUserIntegrationEvent>
    {
        private ILogger _logger;
        private readonly PaymentDbContext _context;
        private readonly IMapper _mapper;

        public UpdateUserIntegrationEventHandler(ILogger logger,PaymentDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }
        public async Task Handle(UpdateUserIntegrationEvent @event)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(x => x.Id == @event.UserId);
            if (customer != null)
            {
                customer = _mapper.Map(@event,customer);
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
