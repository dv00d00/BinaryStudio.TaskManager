namespace BinaryStudio.TaskManager.Logic.Domain
{
    public class Invitation : IEntity
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public  Project Project { get; set; }

        public int UserId { get; set; }

        public  User User { get; set; }

        public bool IsInvitationSended { get; set; }

        public bool IsInvitationConfirmed { get; set; }

    }
}
