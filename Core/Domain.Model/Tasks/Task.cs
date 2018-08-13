using Domain.Model.Base;

namespace Domain.Model.Tasks
{
    public class Task : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
    }
}
