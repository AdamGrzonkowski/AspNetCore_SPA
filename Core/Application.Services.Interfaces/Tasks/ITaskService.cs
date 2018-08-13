using Application.Model.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interfaces.Tasks
{
    public interface ITaskService : IService<TaskModel>
    {
        Task<ICollection<TaskModel>> GetAllCompletedAsync();
        Task<ICollection<TaskModel>> GetAllNotCompletedAsync();
    }
}
