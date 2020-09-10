using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking.API.Data;
using Booking.API.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Booking.API.Services
{
  public class BookingRepository : GenericRepository, IBookingRepository
  {
    private readonly BookingContext _context;
    public BookingRepository(BookingContext context) : base(context)
    {
      _context = context;
    }

    public async Task<Model.Booking> Get(int id)
    {
      return await _context.Bookings.Include(bp => bp.BookingPayments).AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Model.Booking>> GetAll()
    {
      return await _context.Bookings.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<Model.Booking>> Search(BookingSearchParams bookingSearchParams)
    {
        var bookings = _context.Bookings.AsNoTracking().OrderByDescending(h => h.Id).AsQueryable();

        if (bookingSearchParams.HotelId != 0)
        {
          bookings = bookings.Where(b => b.HotelId == bookingSearchParams.HotelId);
        }

        if (bookingSearchParams.CustomerName != null)
        {
          bookings = bookings.Where(b => b.CustomerName.ToLower().Contains(bookingSearchParams.CustomerName.ToLower()));
        }

        //** Between check in and check out date.....
        if (bookingSearchParams.CheckIn != DateTime.MinValue && bookingSearchParams.CheckOut != DateTime.MinValue)
        {
          bookings = bookings.Where(b => b.CheckIn >= bookingSearchParams.CheckIn
                                    && b.CheckOut <= bookingSearchParams.CheckOut);
        } 

        return await PagedList<Model.Booking>.CreateAsync(bookings, bookingSearchParams.PageNumber, bookingSearchParams.PageSize);

    }
  }
}