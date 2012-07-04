using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{
    public interface IUserRepository
    {
        void DeleteUser(int userId);

        void CreateUser(User user);

        void UpdateUser(User user);

        User GetById(int userId);

        IEnumerable<User> GetAll();

        bool LogOn(string userName, string password);

        User GetByName(string userName);

        string GetRoleByName(string userName);

        void Update(User user);

        void Delete(int id);
    }

}
