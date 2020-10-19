using System.Collections.Generic;
using System.Threading.Tasks;
using Hotel.API.Helpers;

namespace Hotel.API.Services
{
    public interface IHotelRepository : IGenericRepository<Model.Hotel>
    {
         Task<Model.Hotel> Get(int id);
         Task<PagedList<Model.Hotel>> GetAll(HotelSearchParams hotelParams);
         Task<PagedList<Model.Hotel>> Search(HotelSearchParams hotelSearchParams);
    }
}