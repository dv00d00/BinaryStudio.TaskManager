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
        /// <param name="receiverId">
        ///   The user id.
        /// </param>
        /// <param name="projectId">
        ///   The project id.
        /// </param>
        /// <param name="senderId"> </param>
        void InviteUserInProject(int senderId, int projectId, int receiverId);

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

        /// <summary>
        /// The confirm invitation in project.
        /// </summary>
        /// <param name="invitationId">
        /// The invitation Id.
        /// </param>
        void ConfirmInvitationInProject(int invitationId);

        /// <summary>
        /// The get all invitations to user.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.Invitation].
        /// </returns>
        IEnumerable<Invitation> GetAllInvitationsToUser(int userId);

        /// <summary>
        /// The get project by id.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The BinaryStudio.TaskManager.Logic.Domain.Project.
        /// </returns>
        Project GetProjectById(int projectId);

        /// <summary>
        /// The refuse from participate project.
        /// </summary>
        /// <param name="invitationId">
        /// The invitation id.
        /// </param>
        void RefuseFromParticipateProject(int invitationId);
    }
}