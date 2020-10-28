using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.API.Model
{
    public class BookingPayment
    {
        public int Id { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }
        public DateTime EnteredDateTime { get; set; }
        public string PaymentMode { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
        public int? PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public int? PaymentTypeId { get; set; }
        public PaymentType PaymentType { get; set; }
    }
}