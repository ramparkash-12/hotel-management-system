using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hotel.API.Data;
using Hotel.API.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Hotel.API.Services
{
  public class HotelRepository : GenericRepository, IHotelRepository
  {
    private readonly HotelContext _context;
    public HotelRepository(HotelContext context) : base(context)
    {
      _context = context;
    }

    public async Task<Model.Hotel> Get(int id)
    {
      return await _context.Hotels.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<IEnumerable<Model.Hotel>> GetAll()
    {
      return await _context.Hotels.AsNoTracking().ToListAsync();
    }

    public async Task<PagedList<Model.Hotel>> Search(HotelSearchParams hotelSearchParams)
    {
        var hotels = _context.Hotels.AsNoTracking().OrderByDescending(h => h.Id).AsQueryable();

        if (hotelSearchParams.Name != null)
        {
            hotels = hotels.Where(h => h.Name.ToLower().Contains(hotelSearchParams.Name.ToLower()));
        }

        if (hotelSearchParams.City != null)
        {
            hotels = hotels.Where(h => h.City.ToLower() == hotelSearchParams.City.ToLower());
        }

        return await PagedList<Model.Hotel>.CreateAsync(hotels, hotelSearchParams.PageNumber, hotelSearchParams.PageSize);

    }

  }
}