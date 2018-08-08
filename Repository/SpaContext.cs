using Entities.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class SpaContext : DbContext
    {
        public SpaContext(DbContextOptions<SpaContext> options) : base(options)
        {
        }

        public DbSet<Task> Tasks { get; set; }
    }
}
