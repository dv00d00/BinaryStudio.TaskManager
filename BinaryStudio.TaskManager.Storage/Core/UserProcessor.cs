using System;
using System.Linq;
using System.Web.Security;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{
    public class UserProcessor : IUserProcessor
    {
        private readonly IUserRepository userRepository;

        public UserProcessor(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }


        public void CreateUser(User userAccount)
        {
            userRepository.CreateUser(userAccount);
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

        public User GetCurrentLoginedEmployee(string userName)
        {
            try
            {
                return userRepository.GetByName(userName);
                
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
