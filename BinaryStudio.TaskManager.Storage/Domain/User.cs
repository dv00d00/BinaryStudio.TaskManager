// --------------------------------------------------------------------------------------------------------------------
// <copyright file="User.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the User type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BinaryStudio.TaskManager.Logic.Domain
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// The user.
    /// </summary>
    public class User : IEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// Just the name of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the credentials.
        /// </summary>
        public Credentials Credentials { get; set; }

        //TODO CLEAN ROLES FROM USERS
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the user projects.
        /// </summary>
        public virtual ICollection<Project> UserProjects { get; set; }

        /// <summary>
        /// Gets or sets the linked in id.
        /// </summary>
        public string LinkedInId { get; set; }

        /// <summary>
        /// Gets or sets the image data.
        /// </summary>
        public byte[] ImageData { get; set; }

        /// <summary>
        /// Gets or sets the image mime tipe.
        /// </summary>
        [HiddenInput(DisplayValue = false)]
        public string ImageMimeType { get; set; }
    }

    /// <summary>
    /// The credentials.
    /// </summary>
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
