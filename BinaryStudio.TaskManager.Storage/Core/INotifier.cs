
namespace BinaryStudio.TaskManager.Logic.Core
{
    public interface INotifier
    {
        void Broadcast(string message);

        void Send(ClientConnection connectionId, string message);
        
        void MoveTask(int taskId, int moveToId);

        void CreateTask(int taskId);

        void SendNews(int newseId);
    }
}