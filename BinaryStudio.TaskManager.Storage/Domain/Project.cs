using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BinaryStudio.TaskManager.Logic.Domain
{
    public class Project :IEntity
    {
        /// <summary>
        /// Entity unique identifier
        /// </summary>
        public int Id {get; set; }

        /// <summary>
        /// Gets or sets the creator id.
        /// </summary>
        /// <value>
        /// The creator id.
        /// </value>
        [Required]
        [ForeignKey("User")]
        public int CreatorId { get; set; }

        public DateTime DateTimeCreation { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
        [ForeignKey("ProjectsAndUsers")]
        public int ProjectsAndUsersId { get; set; }

        public IList<HumanTask> ProjectTasks { get; set; }
    }
}
