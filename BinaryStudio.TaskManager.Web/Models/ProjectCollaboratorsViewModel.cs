namespace BinaryStudio.TaskManager.Web.Models
{
    using System.Collections.Generic;

    using BinaryStudio.TaskManager.Logic.Domain;

    public class ProjectCollaboratorsViewModel
    {
        public IEnumerable<User> Collaborators { get; set; }
    }
}