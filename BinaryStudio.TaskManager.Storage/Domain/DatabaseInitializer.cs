using System.Data.Entity;

namespace BinaryStudio.TaskManager.Logic.Domain
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DataBaseContext>
    {
        protected override void Seed(DataBaseContext context)
        {
            context.Employees.Add(new Employee {Name = "Test1"});
            context.Employees.Add(new Employee {Name = "Test2"});
            context.Employees.Add(new Employee {Name = "Test3"});
            context.Employees.Add(new Employee {Name = "Test4"});

            context.SaveChanges();
        }
    }
}