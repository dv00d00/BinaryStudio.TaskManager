namespace BinaryStudio.TaskManager.Web.Models
{
    using System.Collections.Generic;

    using BinaryStudio.TaskManager.Logic.Domain;

    public class ProjectView
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public IEnumerable<HumanTask> Tasks { get; set; }
    }
}