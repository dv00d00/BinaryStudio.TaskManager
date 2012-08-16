namespace BinaryStudio.TaskManager.Extensions.Models
{
    using System;

    public class LandingTaskModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Priority { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Assigned { get; set; }

        public DateTime? Finished { get; set; }

        public DateTime? Closed { get; set; }

        public string Creator { get; set; }

        public int? AssigneeId { get; set; }

        public string Assignee { get; set; }

        public bool AssigneePhoto { get; set; }
    }
}
