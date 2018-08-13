using Interfaces.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Tasks
{
    public class TaskRepository : BaseRepository<Entities.Tasks.Task>, ITaskRepository
    {
        public TaskRepository(SpaContext context) : base(context)
        {
        }

        public async Task<ICollection<Entities.Tasks.Task>> GetAllCompletedAsync()
        {
            return await GetAll().Where(x => x.Completed).ToListAsync();
        }

        public async Task<ICollection<Entities.Tasks.Task>> GetAllNotCompletedAsync()
        {
            return await GetAll().Where(x => x.Completed == false).ToListAsync();
        }
    }
}
