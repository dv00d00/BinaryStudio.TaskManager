using BinaryStudio.TaskManager.Web.SignalR;

namespace BinaryStudio.TaskManager.Logic.Core
{

    public class Notifier : INotifier
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
            
            this._taskHub.MoveTask(taskId,moveToId,senderConnectionId);
        }
    }
}