using System.Collections.Generic;

namespace BinaryStudio.TaskManager.Logic.Domain
{
    /// <summary>
    /// This model contain information about permissions 
    /// </summary>
    public class UserRoles : IEntity
    {
        /// <summary>
        /// Entity unique identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>
        /// The name of the role. This name must descript roles.
        /// </value>
        public string RoleName { get; set; }

        public virtual ICollection<Permissions> Permissions { get; set; }
    }
}
