using System.Threading.Tasks;

namespace Booking.API.Services
{
    public interface IGenericRepository
    {
        void Insert<T>(T obj) where T : class;
        void Update<T>(T obj) where T : class;
        void Delete<T>(T obj) where T : class;
        Task<bool> SaveAll();
    }
}