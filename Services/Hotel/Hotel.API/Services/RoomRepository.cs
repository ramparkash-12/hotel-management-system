using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hotel.API.Data;
using Hotel.API.Helpers;
using Hotel.API.Model;
using Microsoft.EntityFrameworkCore;

namespace Hotel.API.Services
{
  public class RoomRepository : GenericRepository, IRoomRespository
  {
    private readonly HotelContext _context;
    public RoomRepository(HotelContext context) : base(context)
    {
      _context = context;
    }
    

    public async Task<Room> Get(int id)
    {
      return await _context.Rooms.AsNoTracking().Include(f => f.RoomFacilities).Include(rt => rt.RoomType).FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<IEnumerable<Room>> GetAll()
    {
      return await _context.Rooms.AsNoTracking().Include(f => f.RoomFacilities).Include(rt => rt.RoomType).ToListAsync();
    }

    public async Task<IEnumerable<Room>> Search(RoomSearchParams roomSearchParams)
    {
        var rooms = _context.Rooms.AsNoTracking().OrderByDescending(r => r.Id).AsQueryable();

        if (roomSearchParams.HotelId != 0)
        {
            rooms = rooms.Where(r => r.HotelId == roomSearchParams.HotelId);
        }

        if (roomSearchParams.RoomTypeId != 0)
        {
            rooms = rooms.Where(r => r.RoomTypeId == roomSearchParams.RoomTypeId);
        }

        if (roomSearchParams.isAvailable)
        {
            rooms = rooms.Where(r => r.Available == roomSearchParams.isAvailable);
        }

        return await PagedList<Room>.CreateAsync(rooms, roomSearchParams.PageNumber, roomSearchParams.PageSize);

    }
  }
}