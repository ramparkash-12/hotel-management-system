using System;
using System.Threading.Tasks;
using Booking.API.IntegrationEvents.Events;
using Booking.API.Services;
using Foundation.EventBus.Abstractions;
using Microsoft.Extensions.Logging;

namespace Booking.API.IntegrationEvents.EventHandling
{
    public class CustomerDetailsChangedIntegrationEventHandler : IIntegrationEventHandler<CustomerDetailsChangedIntegrationEvent>
    {
        private readonly ILogger<CustomerDetailsChangedIntegrationEventHandler> _logger;
        private readonly IBookingRepository _repository;

        public CustomerDetailsChangedIntegrationEventHandler(
            ILogger<CustomerDetailsChangedIntegrationEventHandler> logger,
            IBookingRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Handle(CustomerDetailsChangedIntegrationEvent @event)
        {
            _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

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