using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{
    public class UserRepository : IUserRepository
    {
        private readonly DataBaseContext dataBaseContext;


        public UserRepository(DataBaseContext dataBaseContext)
        {
            this.dataBaseContext = dataBaseContext;
        }


        public void DeleteUser(int userId)
        {
            throw new NotImplementedException();
        }

        public void CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public User GetById(int userId)
        {
            throw new NotImplementedException();
        }

        public bool LogOn(string userName, string password)
        {
            IList<User> users = dataBaseContext.Users.ToList();
            foreach (User user in users)
            {
                if(user.UserName == userName && user.Password == password)
                {
                    return true;
                }
            }
            return false;
        }

        public User GetByName(string userName)
        {
            return this.dataBaseContext.Users.ToList().Single(it => it.UserName == userName);
        }
    }
}
