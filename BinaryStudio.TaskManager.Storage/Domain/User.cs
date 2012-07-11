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

        //TODO: refactor this for HASH
        public string Password { get; set; }
        
        public int RoleId { get; set; }

        public ICollection<Project> Projects { get; set; }        

        public ICollection<Project> CreatedProjects { get; set; }
    }
}
