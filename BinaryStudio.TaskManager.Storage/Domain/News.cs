namespace BinaryStudio.TaskManager.Logic.Domain
{
    public class News : IEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public virtual HumanTaskHistory HumanTaskHistory { get; set; }

        public int HumanTaskHistoryId { get; set; }

        public bool IsRead { get; set; }
    }
}
