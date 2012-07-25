namespace BinaryStudio.TaskManager.Logic.Core
{
    using BinaryStudio.TaskManager.Logic.Domain;

    public interface IProjectProcessor
    {
        void CreateDefaultProject(User user);
    }
}