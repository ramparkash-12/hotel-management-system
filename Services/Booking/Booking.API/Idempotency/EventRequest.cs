using System;

namespace Booking.API.Idempotency
{
    public class EventRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
    }
}