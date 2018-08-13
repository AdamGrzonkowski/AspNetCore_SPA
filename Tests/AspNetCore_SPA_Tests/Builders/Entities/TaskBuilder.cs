using AspNetCore_SPA_Tests.Builders.Entities;
using Domain.Model.Tasks;

namespace AspNetCore_SPA_Tests.Builders
{
    public class TaskBuilder : BaseEntityBuilder<TaskBuilder, Task>
    {
        private string Name { get; set; }
        private string Description { get; set; }
        private bool Completed { get; set; }

        public override Task Build()
        {
            Task task = base.Build();

            task.Name = Name;
            task.Description = Description;
            task.Completed = Completed;

            return task;
        }

        public TaskBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public TaskBuilder WithDescription(string description)
        {
            Description = description;
            return this;
        }

        public TaskBuilder WithCompleted(bool completed)
        {
            Completed = completed;
            return this;
        }
    }
}
