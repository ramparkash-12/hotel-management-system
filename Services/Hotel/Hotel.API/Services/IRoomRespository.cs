using System.Collections.Generic;
using System.Threading.Tasks;
using Hotel.API.Helpers;
using Hotel.API.Model;

namespace Hotel.API.Services
{
    public interface IRoomRespository : IGenericRepository
    {
         Task<Room> Get(int id);
         Task<IEnumerable<Room>> GetAll(); 
         Task<IEnumerable<Room>> Search(RoomSearchParams hotelSearchParams);
    }
}