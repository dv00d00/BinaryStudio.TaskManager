
namespace BinaryStudio.TaskManager.Logic.Core
{
    public interface INotifier
    {
        void MoveTask(int taskId, int moveToId);

        void CreateTask(int taskId);

        void SendNews(int newseId);

        void SetCountOfNewses(string userName);
    }
}