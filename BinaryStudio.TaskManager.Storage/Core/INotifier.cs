namespace BinaryStudio.TaskManager.Logic.Core
{
    public interface INotifier
    {
        void Broadcast(string message);

        void Send(ClientConnection connectionId, string message);
    }
}