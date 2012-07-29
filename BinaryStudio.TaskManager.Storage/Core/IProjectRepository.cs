namespace BinaryStudio.TaskManager.Logic.Core
{
    using System.Collections.Generic;

    using BinaryStudio.TaskManager.Logic.Domain;

    /// <summary>
    /// The ProjectRepository interface.
    /// </summary>
    public interface IProjectRepository
    {
        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.Project].
        /// </returns>
        IEnumerable<Project> GetAll();

        /// <summary>
        /// The get by id.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The BinaryStudio.TaskManager.Logic.Domain.Project.
        /// </returns>
        Project GetById(int projectId);

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
        /// The get all projects for user.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.Project].
        /// </returns>
        IEnumerable<Project> GetAllProjectsForUser(int userId);


        /// <summary>
        /// Gets all projects for their creator.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>IEmunerable list of Projects, created by the user.</returns>
        IEnumerable<Project> GetAllProjectsForTheirCreator(int userId);

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="project">
        /// The project.
        /// </param>
        void Add(Project project);

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        void Delete(int projectId);

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="project">
        /// The project.
        /// </param>
        void Update(Project project);

        /// <summary>
        /// The create invitation user in project.
        /// </summary>
        /// <param name="invitation">
        /// The invitation.
        /// </param>
        void AddInvitation(Invitation invitation);

        /// <summary>
        /// The update invitation.
        /// </summary>
        /// <param name="invitation">
        /// The invitation.
        /// </param>
        void UpdateInvitation(Invitation invitation);

        /// <summary>
        /// The get all invitations for user.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.Invitation].
        /// </returns>
        IEnumerable<Invitation> GetAllInvitationsForUser(int userId);

        /// <summary>
        /// The get invitation by id.
        /// </summary>
        /// <param name="invitationId">
        /// The invitation id.
        /// </param>
        /// <returns>
        /// The BinaryStudio.TaskManager.Logic.Domain.Invitation.
        /// </returns>
        Invitation GetInvitationById(int invitationId);
    }
}