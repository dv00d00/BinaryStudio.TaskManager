using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{
    public interface IUserProcessor
    {
        void CreateUser(User user);

        void CreateEmployee(Employee employee);

        void ConnectUserWithEmployee(int userId, int employeeId);

        void SetRoleToUser(string userName, string roleName);

        void SetRoleToUserFromDB(string userName);

        bool IsAdmin(string userName);

        bool LogOnUser(string userName, string password);

        Employee GetCurrentLoginedEmployee(string userName);
    }

}
