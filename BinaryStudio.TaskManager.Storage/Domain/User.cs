using System;

namespace BinaryStudio.TaskManager.Logic.Domain
{
    using System.Collections.Generic;

    public class User : IEntity
    {
        /// <summary>
        /// Entity unique identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// Just the name of the user.
        /// </value>
        public string UserName { get; set; }        

        public string Email { get; set; }
        
        public Credentials Credentials { get; set; }

        //TODO CLEAN ROLES FROM USERS
        public int RoleId { get; set; }

        public virtual ICollection<ProjectsAndUsers> UserProjects { get; set; }

        public string LinkedInId { get; set; }


    }

    public class Credentials
    {
        /// <summary>
        /// Gets or sets the salt.
        /// </summary>
        /// <value>
        /// The salt.
        /// </value>
        public string Salt { get; set; }


        /// <summary>
        /// Gets or sets the password hash.
        /// </summary>
        /// <value>
        /// The password hash.
        /// </value>
        public string Passwordhash { get; set; }

        /// <summary>
        /// Flag for verify user via email (bot security)
        /// </summary>
        public bool IsVerify { get; set; }
    }
}
