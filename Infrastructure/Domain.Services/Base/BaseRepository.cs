using Domain.Model.Base;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository.Base
{
    /// <inheritdoc />
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected SpaContext _context; // Core's DI takes care about disposing it

        public BaseRepository(SpaContext context)
        {
            _context = context;
        }

        public Guid Add(T t)
        {
            DateTime dt = DateTime.Now;
            t.InsTs = dt;
            t.UpdTs = dt;

            _context.Set<T>().Add(t);
            return t.Id;
        }

        public void Delete(T entity)
        {
            // do not remove entirely. Instead, use deferred deletion and only mark as deleted.
            entity.IsDeleted = true;
            entity.UpdTs = DateTime.Now;

            _context.Entry(entity).Property(x => x.IsDeleted).IsModified = true;
            _context.Entry(entity).Property(x => x.UpdTs).IsModified = true;
        }

        public async Task DeleteAsync(Guid id)
        {
            T entity = await FindAsync(x => x.Id == id);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> match)
        {
            return await GetAll().AnyAsync(match);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await GetAll().SingleOrDefaultAsync(match);
        }

        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
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

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T t)
        {
            if (t == null)
            {
                return;
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
            }
        }
    }
}