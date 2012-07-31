namespace BinaryStudio.TaskManager.Web.Models
{
    using System.Collections.Generic;

    using BinaryStudio.TaskManager.Logic.Domain;

    public class ProjectCollaboratorsViewModel
    {
        public IEnumerable<User> PossibleCollaborators { get; set; }

        public IEnumerable<User> Collaborators { get; set; }

        public IEnumerable<User> AlreadyInvited { get; set; }

        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        public int ProjectId { get; set; }
    }
}