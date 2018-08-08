using Entities.Tasks;
using GlobalConfiguration;
using Microsoft.EntityFrameworkCore;
using System;

namespace Repository
{
    public class SpaContext : DbContext
    {
        private readonly IGlobalConfig _globalConfig;

        public SpaContext(DbContextOptions<SpaContext> options, IGlobalConfig globalConfig) : base(options)
        {
            _globalConfig = globalConfig;
        }

        public DbSet<Task> Tasks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (!_globalConfig.IsDevEnv)
            {
                return;
            }

            DateTime dt = DateTime.Now;

            modelBuilder.Entity<Task>().HasData(
                new Task {
                    Id = Guid.NewGuid(),
                    Name = "Buy groceries",
                    Description = "Eggs, bread etc.",
                    Completed = false,
                    InsTs = dt,
                    UpdTs = dt
                },
                new Task
                {
                    Id = Guid.NewGuid(),
                    Name = "Go to gym",
                    Description = "Move your lazy *** :-)",
                    Completed = false,
                    InsTs = dt,
                    UpdTs = dt
                });
        }
    }
}
