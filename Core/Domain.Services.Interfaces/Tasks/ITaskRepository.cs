using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces.Tasks
{
    public interface ITaskRepository : IRepository<Domain.Model.Tasks.Task>
    {
        Task<ICollection<Domain.Model.Tasks.Task>> GetAllCompletedAsync();
        Task<ICollection<Domain.Model.Tasks.Task>> GetAllNotCompletedAsync();
    }
}
