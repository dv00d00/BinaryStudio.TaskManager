// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserRepository.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the UserRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BinaryStudio.TaskManager.Logic.Core
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using BinaryStudio.TaskManager.Logic.Domain;

    /// <summary>
    /// The user repository.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// The data base context.
        /// </summary>
        private readonly DataBaseContext dataBaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="dataBaseContext">
        /// The data base context.
        /// </param>
        public UserRepository(DataBaseContext dataBaseContext)
        {
            this.dataBaseContext = dataBaseContext;
        }

        /// <summary>
        /// The delete user.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        public void DeleteUser(int userId)
        {
            User user = this.dataBaseContext.Users.Single(x => x.Id == userId);
            this.dataBaseContext.Users.Remove(user);
            this.dataBaseContext.SaveChanges();
        }

        /// <summary>
        /// The create new user.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        public void CreateUser(User user)
        {
            this.dataBaseContext.Entry(user).State = EntityState.Added;
            this.dataBaseContext.SaveChanges();
        }

        /// <summary>
        /// The update user.
        /// </summary>
        /// <param name="user">
        /// The user, which will be updated
        /// </param>
        public void UpdateUser(User user)
        {
            this.dataBaseContext.Entry(user).State = EntityState.Modified;
            this.dataBaseContext.SaveChanges();
        }

        /// <summary>
        /// The get by id.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The BinaryStudio.TaskManager.Logic.Domain.User.
        /// </returns>
        public User GetById(int userId)
        {
            return this.dataBaseContext.Users.Single(x => x.Id == userId);
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.User].
        /// </returns>
        public IEnumerable<User> GetAll()
        {
            return this.dataBaseContext.Users.ToList();
        }

        /// <summary>
        /// The get by name.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <returns>
        /// The BinaryStudio.TaskManager.Logic.Domain.User.
        /// </returns>
        public User GetByName(string userName)
        {
            try
            {
                return this.dataBaseContext.Users.ToList().Single(it => it.UserName == userName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// The get role by name.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public string GetRoleByName(string userName)
        {
            return this.dataBaseContext.Roles.ToList().Where(it => it.Id == this.GetByName(userName).RoleId).Select(x => x.RoleName).First();
        }
    }
}
