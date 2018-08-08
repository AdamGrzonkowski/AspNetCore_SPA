using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.Tasks
{
    public class Task : BaseEntity
    {
        // Since it's a very simple application, then validation rules are defined here. 
        // Normally some additional Model classes would be created.
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string Name { get; set; }

        [MinLength(2)]
        public string Description { get; set; }
        public bool Completed { get; set; }
    }
}
