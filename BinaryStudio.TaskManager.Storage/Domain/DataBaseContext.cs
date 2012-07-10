using System.Data.Entity;

namespace BinaryStudio.TaskManager.Logic.Domain
{
    public class DataBaseContext : DbContext
    {
        public DbSet<HumanTask> HumanTasks { get; set; }

        public DbSet<Reminder> Reminders { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserRoles> Roles{ get; set; }

        public DbSet<Permissions> Permissions { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<ProjectsAndUsers> ProjectsAndUserses { get; set; }
    }
}