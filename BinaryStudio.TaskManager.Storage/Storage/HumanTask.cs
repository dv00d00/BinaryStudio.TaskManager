namespace BinaryStudio.TaskManager.Logic.Storage
{
    using System;

    public class HumanTask
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Priority { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Assigned { get; set; }

        public DateTime? Finished { get; set; }

        public DateTime? Closed { get; set; }

        public int? CreatorId { get; set; }

        // public Employee Creator { get; set; }

        public int? AssigneeId { get; set; }

        // public Employee Assignee { get; set; }
    }
}