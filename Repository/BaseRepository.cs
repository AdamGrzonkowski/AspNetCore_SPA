using Entities.Base;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository
{
    /// <inheritdoc />
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private bool _disposed = false;
        protected SpaContext _context;

        public BaseRepository(SpaContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T t)
        {
            DateTime dt = DateTime.Now;
            t.InsTs = dt;
            t.UpdTs = dt;

            _context.Set<T>().Add(t);
            await _context.SaveChangesAsync();
            return t;
        }

        public async Task<int> DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public T Find(Expression<Func<T, bool>> match)
        {
            return GetAll().SingleOrDefault(match);
        }

        public async Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            return await GetAll().Where(predicate).ToListAsync();
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await GetAll().ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<T> UpdateAsync(T t, Guid id)
        {
            if (t == null)
            {
                return null;
            }

            T entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Entry(entity).CurrentValues.SetValues(t);
                entity.UpdTs = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            return entity;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
