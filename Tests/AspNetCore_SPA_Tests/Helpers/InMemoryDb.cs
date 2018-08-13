using AspNetCore_SPA_Tests.Builders;
using Microsoft.EntityFrameworkCore;
using Repository;
using System;

namespace AspNetCore_SPA_Tests.Helpers
{
    /// <summary>
    /// In-memory db, for tests purposes.
    /// </summary>
    public static class InMemoryDb
    {
        public static SpaContext GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<SpaContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .Options;
            var context = new SpaContext(options);

            context.Tasks.AddRange(
                new TaskBuilder().WithName("Some name1").WithDescription("Test desc1").Build(),
                new TaskBuilder().WithName("Some name2").WithDescription("Test desc2").Build(),
                new TaskBuilder().WithName("Completed task3").WithDescription("Test desc5").WithCompleted(true).Build(),
                new TaskBuilder().WithName("Some name4").WithDescription("Test desc4").Build(),
                new TaskBuilder().WithName("Deleted task5").WithDescription("Test desc5").WithDeleted(true).Build());

            context.SaveChanges();

            return context;
        }
    }
}
