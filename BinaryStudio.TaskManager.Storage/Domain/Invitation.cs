namespace BinaryStudio.TaskManager.Logic.Domain
{
    public class Invitation : IEntity
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public  Project Project { get; set; }

        public int SenderId { get; set; }

        public  User Sender { get; set; }

        public int ReceiverId { get; set; }

        public User Receiver { get; set; }

        public bool IsInvitationSended { get; set; }

        public bool IsInvitationConfirmed { get; set; }

    }
}
