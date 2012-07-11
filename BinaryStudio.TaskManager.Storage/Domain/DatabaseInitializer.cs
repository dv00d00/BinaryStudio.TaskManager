using System.Data.Entity;

namespace BinaryStudio.TaskManager.Logic.Domain
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DataBaseContext>
    {
        protected override void Seed(DataBaseContext context)
        {
            context.Roles.Add(new UserRoles { Id = 1, RoleName = "admin" });
            context.Roles.Add(new UserRoles { Id = 2, RoleName = "simpleEmployee" });
            context.Users.Add(new User { Id = 1, UserName = "admin", Email = "admin@mail.ru", Password = "password", RoleId = 1 });
            context.Users.Add(new User { Id = 2, UserName = "simple", Email = "simple@mail.ru", Password = "password", RoleId = 2 });
            context.SaveChanges();
        }
    }
}