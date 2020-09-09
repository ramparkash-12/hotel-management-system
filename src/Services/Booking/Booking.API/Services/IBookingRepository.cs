using System.Collections.Generic;
using System.Threading.Tasks;
using Booking.API.Helpers;
using Booking.API.Model;
using Booking.API.Services;

namespace Booking.API.Services
{
  public interface IBookingRepository : IGenericRepository
  {
    Task<Model.Booking> Get(int id);
    Task<IEnumerable<Model.Booking>> GetAll();
    Task<IEnumerable<Model.Booking>> Search(BookingSearchParams bookingSearchParams);
  }
}