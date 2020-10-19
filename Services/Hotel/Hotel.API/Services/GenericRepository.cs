using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Hotel.API.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Hotel.API.Services
{
   public class GenericRepository<T> : IGenericRepository<T> where T: class
    {
        protected readonly HotelContext _context;
        public GenericRepository(HotelContext context)
        {
            _context = context;
        }

        public void Insert(T entity)
        {
            try
            {
                _context.Set<T>().Add(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }

        public void Update(T entity)
        {
            try
            {
                 _context.Set<T>().Update(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
            }
        }

        public void Delete(T entity)
        {
            try
            {
                 _context.Set<T>().Remove(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be removed: {ex.Message}");
            }
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            try
            {
                 _context.Set<T>().RemoveRange(entities);
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entities)} could not be removed: {ex.Message}");
            }
        }

        public async Task<bool> SaveAll()
        {
            try
            {
                 return await _context.SaveChangesAsync() > 0;
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"Error occured while saving: {sqlEx.Message}");
            }
        }
  }
}