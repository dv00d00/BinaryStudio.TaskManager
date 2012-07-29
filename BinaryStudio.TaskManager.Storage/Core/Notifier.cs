using System.Runtime.Remoting.Contexts;
using BinaryStudio.TaskManager.Web.SignalR;
using SignalR;
using SignalR.Hubs;

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

        public void MoveTask(int taskId, int moveToId)
        {
            moveToId = moveToId == -1 ? 0 : moveToId;
            var context = GlobalHost.ConnectionManager.GetHubContext<TaskHub>();
            context.Clients.TaskMoved(taskId, moveToId);
        }
    }
}