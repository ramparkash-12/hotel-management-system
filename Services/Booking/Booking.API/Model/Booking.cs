using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.API.Model
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalFee { get; set; }
        public int HotelId { get; set; }
        public ICollection<BookingPayment> BookingPayments { get; set; }
        public DateTime EnteredDateTime { get; set; }

        public Booking()
        {
            EnteredDateTime = DateTime.Now;
            BookingPayments = new List<BookingPayment>();
        }
        
    }
}