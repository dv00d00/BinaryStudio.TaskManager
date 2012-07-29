namespace BinaryStudio.TaskManager.Logic.Domain
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The project.
    /// </summary>
    public class Project : IEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the tasks.
        /// </summary>
        public virtual ICollection<HumanTask> Tasks { get; set; }


        /// <summary>
        /// Gets or sets the creator.
        /// </summary>
        public virtual User Creator { get; set; }


        /// <summary>
        /// Gets or sets the creator id.
        /// </summary>
        public int CreatorId { get; set; }

        /// <summary>
        /// Gets or sets the project users.
        /// </summary>
        public virtual ICollection<User> ProjectUsers { get; set; }
    }
}
