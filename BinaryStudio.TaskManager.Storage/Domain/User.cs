﻿namespace BinaryStudio.TaskManager.Logic.Domain
{
    /// <summary>
    ///User
    /// </summary>
    public class User: IEntity
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


        //TODO: delete
        public int RoleId { get; set; }

    }
}
