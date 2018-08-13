using Application.Model.Tasks;
using Application.Services.Base;
using Application.Services.Interfaces.Tasks;
using Domain.Model.Tasks;

namespace Application.Services.Tasks
{
    public class TaskMapper : BaseMapper<TaskModel, Task>, ITaskMapper
    {
        public override TaskModel EntityToModel(Task entity)
        {
            return new TaskModel
            {
                Id = entity.Id,
                Completed = entity.Completed,
                Description = entity.Description,
                Name = entity.Name
            };
        }

        public override Task ModelToEntity(TaskModel model)
        {
            return new Task
            {
                Id = model.Id,
                Name = model.Name,
                Completed = model.Completed,
                Description = model.Description
            };
        }
    }
}
