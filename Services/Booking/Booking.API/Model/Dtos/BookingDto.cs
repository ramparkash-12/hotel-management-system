using System;
using System.ComponentModel.DataAnnotations;

namespace Booking.API.Model.Dtos
{
    public class BookingDto
    {
        [Required]
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        [Required]
        public decimal TotalFee { get; set; }
        public int HotelId { get; set; }
        public int PaymentTypeId { get; set; }
        [Required(ErrorMessage = "Please enter the card holder's name")]
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryMM { get; set; }
        public string ExpiryYY { get; set; }
        public int cvv { get; set; }
    }
}