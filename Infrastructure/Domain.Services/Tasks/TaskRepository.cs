using Interfaces.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Tasks
{
    public class TaskRepository : BaseRepository<Domain.Model.Tasks.Task>, ITaskRepository
    {
        public TaskRepository(SpaContext context) : base(context)
        {
        }

        public async Task<ICollection<Domain.Model.Tasks.Task>> GetAllCompletedAsync()
        {
            return await GetAll().Where(x => x.Completed).ToListAsync();
        }

        public async Task<ICollection<Domain.Model.Tasks.Task>> GetAllNotCompletedAsync()
        {
            return await GetAll().Where(x => x.Completed == false).ToListAsync();
        }
    }
}
