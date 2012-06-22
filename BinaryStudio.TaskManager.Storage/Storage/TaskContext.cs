namespace BinaryStudio.TaskManager.Logic.Storage
{
    using System.Data.Entity;

    public class TaskContext : DbContext
    {
        public DbSet<HumanTask> HumanTasks { get; set; }
    }
}