using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hotel.API.Services
{
    public interface IGenericRepository<T> where T: class
    {
        void Insert (T entity);
        void Update (T entity);
        void Delete (T entity);
        void DeleteRange(IEnumerable<T> entities);
        Task<bool> SaveAll();
    }
}