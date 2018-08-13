using Domain.Model.Base;
using System;

namespace AspNetCore_SPA_Tests.Builders.Entities
{
    public class BaseEntityBuilder<TBuilder, TEntity> 
        where TBuilder : BaseEntityBuilder<TBuilder, TEntity> 
        where TEntity : BaseEntity, new()
    {
        private bool IsDeleted { get; set; }
        private DateTime? InsTs { get; set; }
        private DateTime? UpdTs { get; set; }

        public virtual TEntity Build()
        {
            DateTime dt = DateTime.Now;

            return new TEntity
            {
                Id = Guid.NewGuid(),
                InsTs = InsTs ?? dt,
                UpdTs = UpdTs ?? dt,
                IsDeleted = IsDeleted
            };
        }
        
        public TBuilder WithDeleted(bool isDeleted)
        {
            IsDeleted = isDeleted;
            return (TBuilder)this;
        }

        public TBuilder WithInsTs(DateTime dt)
        {
            InsTs = dt;
            return (TBuilder)this;
        }

        public TBuilder WithUpdTs(DateTime dt)
        {
            UpdTs = dt;
            return (TBuilder)this;
        }
    }
}
