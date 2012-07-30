// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUserRepository.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the IUserRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;

namespace BinaryStudio.TaskManager.Logic.Core
{
    using System.Collections.Generic;

    using BinaryStudio.TaskManager.Logic.Domain;

    /// <summary>
    /// The UserRepository interface.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// The delete user.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        void DeleteUser(int userId);

        /// <summary>
        /// The create user.
        /// </summary>
        /// <param name="user">
        /// The user, which will be created
        /// </param>
        void CreateUser(User user);

        /// <summary>
        /// The update user.
        /// </summary>
        /// <param name="user">
        /// The user, which will be modified
        /// </param>
        void UpdateUser(User user);

        /// <summary>
        /// The get by id.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The BinaryStudio.TaskManager.Logic.Domain.User.
        /// </returns>
        User GetById(int userId);

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.User].
        /// </returns>
        IEnumerable<User> GetAll();

        /// <summary>
        /// The get by name.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <returns>
        /// The BinaryStudio.TaskManager.Logic.Domain.User.
        /// </returns>
        User GetByName(string userName);

        /// <summary>
        /// The get role by name.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        string GetRoleByName(string userName);

        void AddNews(News news);

        IEnumerable<News> GetAllNewsForUser(int userId);


    }
}
