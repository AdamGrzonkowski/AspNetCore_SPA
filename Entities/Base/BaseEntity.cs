using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.Base
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime InsTs { get; set; }
        public DateTime UpdTs { get; set; }
    }
}
