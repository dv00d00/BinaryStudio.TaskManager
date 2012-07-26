namespace BinaryStudio.TaskManager.Logic.Domain
{
    public class Invitation : IEntity
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }

        public int SenderId { get; set; }

        public virtual User Sender { get; set; }

        public int ReceiverId { get; set; }

        public virtual User Receiver { get; set; }

        public bool IsInvitationConfirmed { get; set; }

    }
}
