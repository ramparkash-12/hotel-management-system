using System;
using Foundation.EventBus.Events;

namespace Booking.API.IntegrationEvents.Events
{
    public class CustomerDetailsChangedIntegrationEvent : IntegrationEvent
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public Guid RequestId { get; set; }

        public CustomerDetailsChangedIntegrationEvent(int customerId, string customerName, string customerPhone, Guid requestId )
        {
            CustomerId = customerId;
            CustomerName = customerName;
            CustomerPhone = customerPhone;
            RequestId = requestId;
        }
    }
}