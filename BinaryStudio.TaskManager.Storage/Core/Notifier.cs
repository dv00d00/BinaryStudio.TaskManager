using BinaryStudio.TaskManager.Web.SignalR;

namespace BinaryStudio.TaskManager.Logic.Core
{
    class Notifier : INotifier
    {
        private readonly TaskHub _taskHub;

        public Notifier(TaskHub taskHub)
        {
            _taskHub = taskHub;
        }

        public void Broadcast(string message)
        {
            throw new System.NotImplementedException();
        }

        public void Send(ClientConnection connectionId, string message)
        {
            throw new System.NotImplementedException();
        }

        public void MoveTask(int taskId, int moveToId, string senderConnectionId)
        {
            throw new System.NotImplementedException();
        }
    }
}