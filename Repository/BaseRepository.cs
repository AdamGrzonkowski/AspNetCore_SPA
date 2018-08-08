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
        protected SpaContext _context; // Core's DI takes care about disposing it

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
            await SaveAsync();
            return t;
        }

        public async Task<int> DeleteAsync(T entity)
        {
            // do not remove entirely. Instead, use deferred deletion and only mark as deleted.
            entity.IsDeleted = true;
            _context.Entry(entity).Property(x => x.IsDeleted).IsModified = true;

            return await SaveAsync();
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            T entity = await FindAsync(x => x.Id == id);
            if (entity != null)
            {
                return await DeleteAsync(entity);
            }

            return 0;
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> match)
        {
            return await GetAll().AnyAsync(match);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await GetAll().SingleOrDefaultAsync(match);
        }

        public async Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            return await GetAll().Where(predicate).ToListAsync();
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().Where(x => x.IsDeleted == false);
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

        public async Task<T> UpdateAsync(T t)
        {
            if (t == null)
            {
                return null;
            }

            T entity = await _context.Set<T>().FindAsync(t.Id);
            if (entity != null)
            {
                // dedicated mappers should be used instead of this method, as it overwrites all values, but since it's a test application, then we're using this approach.
                _context.Entry(entity).CurrentValues.SetValues(t); 

                // do not modify id and inserted timestamp
                _context.Entry(entity).Property(x => x.Id).IsModified = false;
                _context.Entry(entity).Property(x => x.InsTs).IsModified = false;

                entity.UpdTs = DateTime.Now;
                await SaveAsync();
            }
            return entity;
        }
    }
}
