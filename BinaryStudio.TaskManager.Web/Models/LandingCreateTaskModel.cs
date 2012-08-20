namespace BinaryStudio.TaskManager.Web.Models
{
    using System;

    public class LandingCreateTaskModel
    {
        public string Name { get; set; }

        public int Priority { get; set; }

        public int? AssigneeId { get; set; }

        public int ProjectId { get; set; }

        public DateTime? FinishDate { get; set; }
    }
}