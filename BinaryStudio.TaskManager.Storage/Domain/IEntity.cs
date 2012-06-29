namespace BinaryStudio.TaskManager.Logic.Domain
{
    /// <summary>
    /// Base interface for all database entities
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Entity unique identifier
        /// </summary>
        int Id { get; set; }
    }
}