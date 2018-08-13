using Application.Model.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IService<TModel> where TModel : BaseModel
    {
        Guid Add(TModel t);

        Task<ICollection<TModel>> GetAllAsync();
        Task<TModel> GetByIdAsync(Guid id);

        Task<bool> ExistsAsync(Guid id);

        Task UpdateAsync(TModel t);

        Task DeleteAsync(Guid id);

        /// <summary>
        /// Commits all changes made in the given context (from all services / repositories) and automatically rolls back those changes in case of any errors.
        /// </summary>
        /// <returns></returns>
        Task<int> SaveAsync();
    }
}
