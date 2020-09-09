using System.Threading.Tasks;
using Booking.API.Data;

namespace Booking.API.Services
{
    public class GenericRepository : IGenericRepository
    {
        private readonly BookingContext _context;
        public GenericRepository(BookingContext context)
        {
            _context = context;
        }
        
        public void Insert<T>(T entity) where T : class
        {
            _context.Add(entity);
        }
        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity); 
        }
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public async Task<bool> SaveAll()
        {
        return await _context.SaveChangesAsync() > 0;
        }
  }
}