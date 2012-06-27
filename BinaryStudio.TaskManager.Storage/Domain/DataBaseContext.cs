using System.Data.Entity;

namespace BinaryStudio.TaskManager.Logic.Domain
{
    public class DataBaseContext : DbContext
    {
        public DbSet<HumanTask> HumanTasks { get; set; }

        public DbSet<Reminder> Reminders { get; set; }

        public DbSet<Employee> Employees { get; set; }
    }
}