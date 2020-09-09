using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hotel.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotel.API.Services
{
   public class GenericRepository : IGenericRepository 
    {
        private readonly HotelContext _context;
        public GenericRepository(HotelContext context)
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