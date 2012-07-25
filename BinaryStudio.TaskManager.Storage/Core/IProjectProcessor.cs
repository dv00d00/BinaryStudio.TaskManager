namespace BinaryStudio.TaskManager.Logic.Core
{
    using System.Collections.Generic;

    using BinaryStudio.TaskManager.Logic.Domain;

    /// <summary>
    /// The ProjectProcessor interface.
    /// </summary>
    public interface IProjectProcessor
    {
        /// <summary>
        /// The create default project.
        /// </summary>
        /// <param name="user">
        /// The current user.
        /// </param>
        void CreateDefaultProject(User user);

        /// <summary>
        /// The invite user in project.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        void InviteUserInProject(int userId, int projectId);

        /// <summary>
        /// The remove user from project.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        void RemoveUserFromProject(int userId, int projectId);

        /// <summary>
        /// The get all users in project.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.User].
        /// </returns>
        IEnumerable<User> GetAllUsersInProject(int projectId);
    }
}