using System.Collections.Generic;
using System.Threading.Tasks;
using Hotel.API.Helpers;

namespace Hotel.API.Services
{
    public interface IHotelRepository : IGenericRepository
    {
         Task<Model.Hotel> Get(int id);
         Task<IEnumerable<Model.Hotel>> GetAll();
         Task<IEnumerable<Model.Hotel>> Search(HotelSearchParams hotelSearchParams);
    }
}