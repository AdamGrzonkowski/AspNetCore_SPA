using Application.Model.Tasks;
using Application.Services.Interfaces.Tasks;
using Interfaces.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Tasks
{
    public class TaskService : BaseService<TaskModel, Domain.Model.Tasks.Task, ITaskMapper, ITaskRepository>, ITaskService
    {
        public TaskService(ITaskRepository repository, ITaskMapper mapper) : base(repository, mapper)
        {
        }

        public async Task<ICollection<TaskModel>> GetAllCompletedAsync()
        {
            var entities = await Repository.GetAllCompletedAsync();
            return Mapper.MapEntitiesToModels(entities);
        }

        public async Task<ICollection<TaskModel>> GetAllNotCompletedAsync()
        {
            var entities = await Repository.GetAllNotCompletedAsync();
            return Mapper.MapEntitiesToModels(entities);
        }
    }
}
