using System.Collections.Generic;
using System.Data;
using System.Linq;
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
            User user = this.dataBaseContext.Users.Single(x => x.Id == userId);
            this.dataBaseContext.Users.Remove(user);
            this.dataBaseContext.SaveChanges();
        }

        public void CreateUser(User user)
        {
            this.dataBaseContext.Entry(user).State = EntityState.Added;
            this.dataBaseContext.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            this.dataBaseContext.Entry(user).State = EntityState.Modified;
            this.dataBaseContext.SaveChanges();
        }

        public User GetById(int userId)
        {
            return this.dataBaseContext.Users.Single(x => x.Id == userId);
        }

        public IEnumerable<User> GetAll()
        {
            return this.dataBaseContext.Users.ToList();
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
            var user = this.dataBaseContext.Users.ToList().Single(it => it.UserName.Equals(userName));
            return user;
        }

        public string GetRoleByName(string userName)
        {
            return this.dataBaseContext.Roles.ToList().
                Where(it => it.Id == this.GetByName(userName).RoleId).
                Select(x => x.RoleName).
                First();
        }

        public void Update(User user)
        {
            this.dataBaseContext.Entry(user).State = EntityState.Modified;
            this.dataBaseContext.SaveChanges();
        }

        public void Delete(int id)
        {
            User user = this.dataBaseContext.Users.Single(x => x.Id == id);
            this.dataBaseContext.Users.Remove(user);
            this.dataBaseContext.SaveChanges();
        }
    }
}
