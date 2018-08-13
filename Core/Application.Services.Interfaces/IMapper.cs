using Application.Model.Base;
using Domain.Model.Base;
using System.Collections.Generic;

namespace Application.Services.Interfaces
{
    public interface IMapper<TModel,TEntity>
        where TModel : BaseModel
        where TEntity : BaseEntity
    {
        TModel EntityToModel(TEntity entity);
        TEntity ModelToEntity(TModel model);
        ICollection<TModel> MapEntitiesToModels(ICollection<TEntity> entities);
    }
}
