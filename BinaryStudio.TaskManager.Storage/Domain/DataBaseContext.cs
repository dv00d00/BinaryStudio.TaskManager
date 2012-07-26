using System.Data.Entity;

namespace BinaryStudio.TaskManager.Logic.Domain
{
    public class DataBaseContext : DbContext
    {
        public DbSet<HumanTask> HumanTasks { get; set; }

        public DbSet<Reminder> Reminders { get; set; }        

        public DbSet<User> Users { get; set; }

        public DbSet<UserRoles> Roles{ get; set; }

        public DbSet<Permissions> Permissions { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<HumanTaskHistory> HumanTaskHistories { get; set; }

        public DbSet<Priority> Priorities { get; set; }

        public DbSet<Invitation> Invitations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Project>().HasRequired(x => x.Creator) //or HasOptional
                              .WithMany() //Unidirectional
                              .Map(x => x.MapKey("Creator")) //FK column Name
                              .WillCascadeOnDelete(false);

            modelBuilder.Entity<Invitation>()
                    .HasRequired(e => e.Sender)
                    .WithMany()
                    .HasForeignKey(e => e.SenderId)
                    .WillCascadeOnDelete(false);
         /*   modelBuilder.Entity<Project>().HasKey(it => it.Id);

            modelBuilder.Entity<Project>().HasMany(it => it.Users).WithMany(it => it.Projects);

            modelBuilder.Entity<Project>().HasRequired(it => it.Creator).WithMany(it => it.CreatedProjects).
                HasForeignKey(it => it.CreatorId).WillCascadeOnDelete(false);
    */
            modelBuilder.Entity<User>().HasMany(x => x.UserProjects).WithMany(x => x.ProjectUsers);
        }
    }
}