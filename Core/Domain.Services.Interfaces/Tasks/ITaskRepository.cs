using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces.Tasks
{
    public interface ITaskRepository : IRepository<Entities.Tasks.Task>
    {
        Task<ICollection<Entities.Tasks.Task>> GetAllCompletedAsync();
        Task<ICollection<Entities.Tasks.Task>> GetAllNotCompletedAsync();
    }
}
