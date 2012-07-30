namespace BinaryStudio.TaskManager.Web.Models
{
    using System;
    using System.Collections.Generic;

    using BinaryStudio.TaskManager.Logic.Domain;

    public class LandingIndexModel
    {
        /// <summary>
        /// Gets or sets the user's projects.
        /// </summary>
        /// <value>
        /// The user's projects.
        /// </value>
        public IEnumerable<Project> UserProjects { get; set; }

        /// <summary>
        /// Gets or sets the creator's projects.
        /// </summary>
        /// <value>
        /// The creator's projects.
        /// </value>
        public IEnumerable<Project> CreatorProjects { get; set; }

        public HumanTask ProjectHumanTask { get; set; }
    }

    public class LandingProjectsModel
    {
        public IEnumerable<ProjectView> UserProjects { get; set; }

        public IEnumerable<ProjectView> CreatorProjects { get; set; }
    }

    public class LandingTasksModel
    {
        public IEnumerable<TaskView> Tasks { get; set; }
    }

    public class TaskView
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Priority { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Assigned { get; set; }

        public DateTime? Finished { get; set; }

        public DateTime? Closed { get; set; }
    }
}