using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Model.Base
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime InsTs { get; set; }
        public DateTime UpdTs { get; set; }
        public bool IsDeleted { get; set; }
    }
}
