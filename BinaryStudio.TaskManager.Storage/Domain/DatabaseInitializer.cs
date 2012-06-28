using System.Data.Entity;

namespace BinaryStudio.TaskManager.Logic.Domain
{
    using System;

    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DataBaseContext>
    {
        protected override void Seed(DataBaseContext context)
        {
            context.Employees.Add(new Employee { Name = "Test1" });
            context.Employees.Add(new Employee { Name = "Test2" });
            context.Employees.Add(new Employee { Name = "Test3" });
            context.Employees.Add(new Employee { Name = "Test4" });
            
            foreach (var employee in context.Employees)
            {
                employee.Tasks.Add(new HumanTask { Description = "Hello world1", Name = "Test1", Created = DateTime.Now });
                employee.Tasks.Add(new HumanTask { Description = "Hello world2", Name = "Test2", Created = DateTime.Now });
                employee.Tasks.Add(new HumanTask { Description = "Hello world3", Name = "Test3", Created = DateTime.Now });
                employee.Tasks.Add(new HumanTask { Description = "Hello world4", Name = "Test4", Created = DateTime.Now });
            }

            context.SaveChanges();
        }
    }
}