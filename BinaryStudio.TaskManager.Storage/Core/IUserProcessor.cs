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

        void SetRoleToUser(string userName, string roleName);

        void SetRoleToUserFromDB(string userName);

        bool IsAdmin(string userName);

        bool LogOnUser(string userName, string password);

        User GetCurrentLoginedEmployee(string userName);

        bool CreateUser(string userName, string password, string eMail, string linkeinId);

        User GetUser(int userId);

        User GetUserByLinkedInId(string linkedinId);
    }

}
