using System.Collections.Generic;

namespace Booking.API.Model
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string Expiry { get; set; }
        public int SecurityNumber { get; set; }
        public ICollection<BookingPayment> BookingPayments { get; set; }
    }
}