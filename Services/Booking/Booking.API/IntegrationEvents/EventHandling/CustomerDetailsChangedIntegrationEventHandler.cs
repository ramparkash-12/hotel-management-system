using System;
using System.Threading.Tasks;
using Booking.API.Idempotency;
using Booking.API.IntegrationEvents.Events;
using Booking.API.Services;
using Foundation.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Booking.API.IntegrationEvents.EventHandling
{
    public class CustomerDetailsChangedIntegrationEventHandler : IIntegrationEventHandler<CustomerDetailsChangedIntegrationEvent>
    {
        private readonly ILogger<CustomerDetailsChangedIntegrationEventHandler> _logger;
        private readonly IBookingRepository _repository;
        private readonly IEventRequestManager _requestManager;

        public CustomerDetailsChangedIntegrationEventHandler(
            ILogger<CustomerDetailsChangedIntegrationEventHandler> logger,
            IBookingRepository repository, IEventRequestManager requestManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _requestManager = requestManager ?? throw new ArgumentNullException(nameof(requestManager)); 
        }

        public async Task Handle(CustomerDetailsChangedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventHandlerContext", $"{@event.Id}-{Program.AppName}"))
            {

                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
            
                var alreadyExists = await _requestManager.ExistAsync(@event.RequestId);

                if (!alreadyExists)
                {
                    await _requestManager.SaveEventRequest<CustomerDetailsChangedIntegrationEventHandler>(@event.RequestId);
                    //** Get bookings 
                    var bookings = _repository.GetAll();

                    //** loop through and update matched customers information
                    foreach (var booking in bookings.Result)
                    {
                        if (booking.CustomerId == @event.CustomerId)
                            UpdateCustomerDetails(@event.CustomerId, @event.CustomerName, @event.CustomerPhone, booking);
                    }
                    
                    await _repository.SaveAll();
                }
            }
        }

        private void UpdateCustomerDetails(int customerId, string customerName, string customerPhone, Model.Booking booking)
        {
            _logger.LogInformation("----- CustomerDetailsChangedIntegrationEventHandler - Updating customer infromation....");

            if (!string.IsNullOrWhiteSpace(customerName))
                booking.CustomerName = customerName;

            if (!string.IsNullOrWhiteSpace(customerPhone))
                booking.CustomerPhone = customerPhone;

            _repository.Update(booking);
        }
    }
}