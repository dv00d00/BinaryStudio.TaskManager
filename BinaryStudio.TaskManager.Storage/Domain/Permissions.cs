namespace BinaryStudio.TaskManager.Logic.Domain
{
    public class Permissions : IEntity
    {
        /// <summary>
        /// Entity unique identifier
        /// </summary>
        public int Id { get; set; }

        public bool MayInviteCollaborators { get; set; }

        public bool MayCreateTask { get; set; }

    }
}
