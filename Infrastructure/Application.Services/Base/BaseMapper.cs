using Application.Model.Base;
using Application.Services.Interfaces;
using Domain.Model.Base;
using System.Collections.Generic;

namespace Application.Services.Base
{
    public abstract class BaseMapper<TModel, TEntity> : IMapper<TModel, TEntity>
        where TModel : BaseModel
        where TEntity : BaseEntity
    {
        public ICollection<TModel> MapEntitiesToModels(ICollection<TEntity> entities)
        {
            ICollection<TModel> result = new List<TModel>();
            foreach (var item in entities)
            {
                result.Add(EntityToModel(item));
            }

            return result;
        }

        public abstract TModel EntityToModel(TEntity entity);
        public abstract TEntity ModelToEntity(TModel model);
    }
}
