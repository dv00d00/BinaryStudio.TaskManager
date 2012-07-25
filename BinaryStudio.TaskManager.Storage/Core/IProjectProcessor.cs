namespace BinaryStudio.TaskManager.Logic.Core
{
    using BinaryStudio.TaskManager.Logic.Domain;

    public interface IProjectProcessor
    {
        void CreateDefaultProject(User user);

        void InviteUserInProject(int userId, int projectId);

        void RemoveUserFromProject(int userId, int projectId);
    }
}