using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hotel.API.Data;
using Hotel.API.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Hotel.API.Services
{
  public class HotelRepository : GenericRepository<Model.Hotel>, IHotelRepository
  {
    public HotelRepository(HotelContext context) : base(context)
    {
    }

    public async Task<Model.Hotel> Get(int id)
    {
      return await _context.Hotels.AsNoTracking().Include(hotelImages => hotelImages.Images)
      .FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<PagedList<Model.Hotel>> GetAll(HotelSearchParams hotelParams)
    {
      var hotels = _context.Hotels.AsNoTracking().OrderByDescending(h=> h.Id).AsQueryable();
      
      if (hotelParams.Name != null && !string.IsNullOrWhiteSpace(hotelParams.Name))
      {
          hotels = hotels.Where(h => h.Name.ToLower().Contains(hotelParams.Name.ToLower()));
      }

      if (hotelParams.City != null && !string.IsNullOrWhiteSpace(hotelParams.City))
      {
          hotels = hotels.Where(h => h.City.ToLower() == hotelParams.City.ToLower());
      }

      return await PagedList<Model.Hotel>.CreateAsync(hotels, hotelParams.PageNumber, hotelParams.PageSize);

    }

    public async Task<PagedList<Model.Hotel>> Search(HotelSearchParams hotelSearchParams)
    {
        var hotels = _context.Hotels.AsNoTracking().
                    OrderByDescending(h => h.Id).Include(hr => hr.Rooms).AsQueryable();

        if (hotelSearchParams.City != null && !string.IsNullOrWhiteSpace(hotelSearchParams.City))
        {
            hotels = hotels.Where(h => h.City.ToLower() == hotelSearchParams.City.ToLower());
        }

        /* if (hotelSearchParams.Adults != 0)
        {
            if (hotelSearchParams.Adults == 1 )
              hotelSearchParams.Adults = 2;

            hotels = hotels.Where(h => h.Rooms.Any(r => r.MaximumGuests == hotelSearchParams.Adults));
        } */

        return await PagedList<Model.Hotel>.CreateAsync(hotels, hotelSearchParams.PageNumber, hotelSearchParams.PageSize);

    }

  }
}