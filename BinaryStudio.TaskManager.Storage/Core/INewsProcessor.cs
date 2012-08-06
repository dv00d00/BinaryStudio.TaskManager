using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{
    public interface INewsProcessor
    {
        void CreateNewsForUsersInProject(HumanTaskHistory taskHistory, int projectId);

        void AddNews(HumanTaskHistory humanTaskHistory, User user);

        void AddNews(News news);
    }
}
