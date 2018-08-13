using Application.Model.Tasks;
using Domain.Model.Tasks;

namespace Application.Services.Interfaces.Tasks
{
    public interface ITaskMapper : IMapper<TaskModel, Task>
    {
    }
}
