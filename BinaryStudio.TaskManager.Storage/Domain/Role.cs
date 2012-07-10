using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryStudio.TaskManager.Logic.Domain
{
    /// <summary>
    /// This model contain information about permissions 
    /// </summary>
    public class Role :IEntity
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

        /// <summary>
        /// Gets or sets the permissions list.
        /// </summary>
        /// <value>
        /// The permissions list.
        /// </value>
        public IEnumerable<Permissions> PermissionsList { get; set; }


    }
}
