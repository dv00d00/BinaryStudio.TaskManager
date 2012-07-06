using System;
using System.Linq;
using System.Web.Security;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{
    public class UserProcessor : IUserProcessor
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IUserRepository userRepository;

        public UserProcessor(IUserRepository userRepository, IEmployeeRepository employeeRepository)
        {
            this.userRepository = userRepository;
            this.employeeRepository = employeeRepository;
        }


        public void CreateUser(User userAccount)
        {
            userRepository.CreateUser(userAccount);
        }

        public void CreateEmployee(Employee employee)
        {
            this.CreateEmployee(employee);
        }

        public void ConnectUserWithEmployee(int userId, int employeeId)
        {
            Employee employee = employeeRepository.GetById(employeeId);
            employee.UserId = userId;
            employeeRepository.Update(employee);
        }

        public void SetRoleToUser(string userName, string roleName)
        {
            if (!Roles.RoleExists(roleName))
            {
                Roles.CreateRole(roleName);
            }

            if (!Roles.IsUserInRole(userName, roleName))
            {
                Roles.AddUserToRole(userName, roleName);
            }
        }

        public void SetRoleToUserFromDB(string userName)
        {
            if (!Roles.RoleExists(userRepository.GetRoleByName(userName)))
            {
                Roles.CreateRole(userRepository.GetRoleByName(userName));
            }

            if (!Roles.IsUserInRole(userName, userRepository.GetRoleByName(userName)))
            {
                Roles.AddUserToRole(userName, userRepository.GetRoleByName(userName));
            }
        }

        public bool IsAdmin(string userName)
        {
            return Roles.IsUserInRole("admin");
        }

        public bool LogOnUser(string userName, string password)
        {
            return this.userRepository.LogOn(userName, password);
        }

        public Employee GetCurrentLoginedEmployee(string userName)
        {
            try
            {
                User user = userRepository.GetByName(userName);
                return employeeRepository.GetAll().ToList().Single(it => it.UserId == user.Id);
            }
            catch (Exception)
            {
                return null;
            }
            
        }
    }
}
