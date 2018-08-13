using Domain.Model.Base;
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
        Guid Add(T t);

        IQueryable<T> GetAll();
        Task<ICollection<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> match);
        Task<T> FindAsync(Expression<Func<T, bool>> match);
        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> predicate);

        Task UpdateAsync(T t);

        void Delete(T entity);
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Commits all changes made in the given context and automatically rolls back those changes in case of any errors.
        /// </summary>
        /// <returns></returns>
        Task<int> SaveAsync();
    }
}
