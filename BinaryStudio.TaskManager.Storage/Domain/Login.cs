using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryStudio.TaskManager.Logic.Domain
{
    /// <summary>
    ///Login
    /// </summary>
    public class Login
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// Just the name of the user.
        /// </value>
        public string UserName { get; set; }

        public string Email { get; set; }

        //TODO: refactor this for HASH
        public string Password { get; set; }

        public DateTime Created { get; set; }

        public int? EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>
        /// Id of role
        /// </value>
        public int Role { get; set; }
    }
}
