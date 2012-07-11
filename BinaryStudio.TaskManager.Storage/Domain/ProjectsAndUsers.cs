using System.Collections.Generic;
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
   
        public virtual User User { get; set; }

        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        /// <value>
        /// The project id.
        /// </value>    
        public virtual Project Project { get; set; }

        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        /// <value>
        /// The role id.
        /// </value>        
        public virtual UserRoles Role { get; set; }
    }
}
