using Application.Model.Tasks;
using System;

namespace AspNetCore_SPA_Tests.Builders
{
    public class TaskModelBuilder
    {
        private string Name { get; set; }
        private string Description { get; set; }
        private bool Completed { get; set; }

        public TaskModel Build()
        {
            TaskModel task = new TaskModel
            {
                Id = Guid.NewGuid(),
                Name = Name,
                Description = Description,
                Completed = Completed
            };

            return task;
        }

        public TaskModelBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public TaskModelBuilder WithDescription(string description)
        {
            Description = description;
            return this;
        }

        public TaskModelBuilder WithCompleted(bool completed)
        {
            Completed = completed;
            return this;
        }
    }
}
