using Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Interfaces
{
    /// <summary>
    /// Abstraction over repository / data access layer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> AddAsync(T t);

        IQueryable<T> GetAll();
        Task<ICollection<T>> GetAllAsync();
        Task<T> GetAsync(int id);

        Task<T> FindAsync(Expression<Func<T, bool>> match);
        Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate);

        Task<T> UpdateAsync(T t);

        Task<int> DeleteAsync(T entity);
        Task<int> DeleteAsync(Guid id);

        Task<int> SaveAsync();
    }
}
