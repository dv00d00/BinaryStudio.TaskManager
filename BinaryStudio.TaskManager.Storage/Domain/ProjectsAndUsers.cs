using System.ComponentModel.DataAnnotations;

namespace BinaryStudio.TaskManager.Logic.Domain
{
    public class ProjectsAndUsers : IEntity
    {
        /// <summary>
        /// Entity unique identifier
        /// </summary>
        public int Id {get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>
        /// The user id.
        /// </value>
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        /// <value>
        /// The project id.
        /// </value>
        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        /// <value>
        /// The role id.
        /// </value>
        [ForeignKey("Role")]
        public int RoleId { get; set; }
    }
}
