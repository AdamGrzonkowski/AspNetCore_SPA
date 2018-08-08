using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces.Tasks
{
    public interface ITaskRepository : IRepository<Entities.Tasks.Task>
    {
        Task<ICollection<Entities.Tasks.Task>> GetAllCompleted();
        Task<ICollection<Entities.Tasks.Task>> GetAllNotCompleted();
    }
}
