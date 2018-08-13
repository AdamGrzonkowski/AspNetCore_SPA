using Application.Model.Base;
using System.ComponentModel.DataAnnotations;

namespace Application.Model.Tasks
{
    public class TaskModel : BaseModel
    {
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }
        public bool Completed { get; set; }
    }
}
