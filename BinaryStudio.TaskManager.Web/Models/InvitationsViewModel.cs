namespace BinaryStudio.TaskManager.Web.Models
{
    using System.Collections.Generic;

    using BinaryStudio.TaskManager.Logic.Domain;

    public class InvitationsViewModel
    {
        public User User { get; set; }

        public Project Project { get; set; }

        public Invitation Invitation { get; set; }
    }
}