namespace BinaryStudio.TaskManager.Logic.Core
{
    using System.Collections.Generic;

    using BinaryStudio.TaskManager.Logic.Domain;

    public interface IProjectRepository
    {
        IEnumerable<Project> GetAll();

        Project GetById(int projectId);

        IEnumerable<User> GetAllUsersInProject(int projectId);

        IEnumerable<Project> GetAllProjectsForUser(int userId);

        void Add(Project project);

        void Delete(int projectId);

        void Update(Project project);

        void CreateInvitationUserInProject(Invitation invitation);

        void UpdateInvitation(Invitation invitation);
    }
}