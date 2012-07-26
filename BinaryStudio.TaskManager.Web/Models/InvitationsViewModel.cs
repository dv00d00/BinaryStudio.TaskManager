namespace BinaryStudio.TaskManager.Web.Models
{
    using BinaryStudio.TaskManager.Logic.Domain;

    public class InvitationsViewModel
    {
        public User User { get; set; }

        public Project Project { get; set; }

        public Invitation Invitation { get; set; }
    }
}