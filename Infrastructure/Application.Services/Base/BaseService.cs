using Application.Model.Base;
using Application.Services.Interfaces;
using Domain.Model.Base;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public abstract class BaseService<TModel, TEntity, TMapper, TRepository> : IService<TModel> 
        where TModel : BaseModel
        where TEntity : BaseEntity
        where TMapper : IMapper<TModel, TEntity>
        where TRepository : IRepository<TEntity>
    {
        protected readonly TRepository Repository;
        protected readonly TMapper Mapper;

        protected BaseService(TRepository repository, TMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        public Guid Add(TModel t)
        {
            var entity = Mapper.ModelToEntity(t);
            return Repository.Add(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await Repository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await Repository.ExistsAsync(x => x.Id == id);
        }

        public async Task<ICollection<TModel>> GetAllAsync()
        {
            var entities = await Repository.GetAllAsync();
            return Mapper.MapEntitiesToModels(entities);
        }

        public async Task<TModel> GetByIdAsync(Guid id)
        {
            TEntity entity = await Repository.GetByIdAsync(id);
            return Mapper.EntityToModel(entity);
        }

        public async Task UpdateAsync(TModel t)
        {
            TEntity entity = Mapper.ModelToEntity(t);
            await Repository.UpdateAsync(entity);
        }

        public async Task<int> SaveAsync()
        {
            return await Repository.SaveAsync();
        }
    }
}
