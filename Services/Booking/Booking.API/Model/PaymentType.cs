using System.Collections.Generic;

namespace Booking.API.Model
{
    public class PaymentType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public ICollection<BookingPayment> BookingPayments { get; set; }
    }
}