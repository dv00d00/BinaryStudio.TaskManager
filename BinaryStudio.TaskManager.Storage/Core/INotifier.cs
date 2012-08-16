
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{
    public interface INotifier
    {
        void MoveTask(HumanTask task, int moveToId);

        void CreateTask(int taskId);

        void SetCountOfNews(string userName);

        void SetCountOfNews(int userId);

        void BroadcastNewsToDesktopClient(News news);

        void BroadcastNews(News news);

        bool SendReminderToDesktopClient(int userId, string message);
    }
}