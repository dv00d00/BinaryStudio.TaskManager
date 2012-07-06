using System.Data.Entity;

namespace BinaryStudio.TaskManager.Logic.Domain
{
    using System;

    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DataBaseContext>
    {

        protected override void Seed(DataBaseContext context)
        {
            context.Employees.Add(new Employee { Name = "�������", Id = 1, UserId = 2});
            context.Employees.Add(new Employee { Name = "ϸ��", Id = 2, UserId = 3 });
            context.Employees.Add(new Employee { Name = "����", Id = 3, UserId = 4 });
            context.Employees.Add(new Employee { Name = "����������", Id = 4, UserId = 5 });
            context.Employees.Add(new Employee { Name = "�������", Id = 4, UserId = 1 });

            foreach (var employee in context.Employees)
            {
                employee.Tasks.Add(new HumanTask { Description = "����������", Name = "����", Created = DateTime.Now, AssigneeId = employee.Id, Id = (employee.Id-1)*5+1,Priority = 2});
                employee.Tasks.Add(new HumanTask { Description = "������", Name = "����", Created = DateTime.Now, AssigneeId = employee.Id, Id = (employee.Id - 1) * 5 + 1,Priority = 1});
                employee.Tasks.Add(new HumanTask { Description = "�������� ���-��", Name = "�����", Created = DateTime.Now, AssigneeId = employee.Id, Id = (employee.Id - 1) * 5 + 1,Priority = 0});
                employee.Tasks.Add(new HumanTask { Description = "�������", Name = "����", Created = DateTime.Now, AssigneeId = employee.Id, Id = (employee.Id - 1) * 5 + 1,Priority = 2});
            }

            context.Roles.Add(new UserRoles { Id = 1, RoleName = "admin" });
            context.Roles.Add(new UserRoles { Id = 2, RoleName = "simpleEmployee" });
            context.Users.Add(new User { Id = 1, UserName = "admin", Email = "admin@mail.ru", Password = "password", RoleId = 1 });
            context.Users.Add(new User { Id = 2, UserName = "simple", Email = "simple@mail.ru", Password = "password", RoleId = 2 });
            context.SaveChanges();
        }
    }
}