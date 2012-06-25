using System.Data.Entity;

namespace BinaryStudio.TaskManager.Logic.Domain
{
    public class TaskContext : DbContext
    {
        public DbSet<HumanTask> HumanTasks { get; set; }
    }
}