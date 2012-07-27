namespace BinaryStudio.TaskManager.Web.Models
{
    using System.Collections.Generic;

    using BinaryStudio.TaskManager.Logic.Domain;

    public class LandingIndexModel
    {
        /// <summary>
        /// Gets or sets the projects.
        /// </summary>
        /// <value>
        /// The projects.
        /// </value>
        public IEnumerable<Project> Projects { get; set; }

        public HumanTask ProjectHumanTask { get; set; }
    }

    public class LandingProjectsModel
    {
        public IEnumerable<ProjectView> Projects { get; set; }
    }

    public class ProjectView
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public IEnumerable<HumanTask> Tasks { get; set; }
    }
}