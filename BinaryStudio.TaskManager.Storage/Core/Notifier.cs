using System.Linq;
using System.Runtime.Remoting.Contexts;
using BinaryStudio.TaskManager.Logic.Core.SignalR;
using BinaryStudio.TaskManager.Web.SignalR;
using SignalR;
using SignalR.Hubs;

namespace BinaryStudio.TaskManager.Logic.Core
{

    public class Notifier : INotifier
    {
        private readonly IHumanTaskRepository humanTaskRepository;

        private readonly TaskHub _taskHub;

        public Notifier(TaskHub taskHub, IHumanTaskRepository humanTaskRepository)
        {
            _taskHub = taskHub;
            this.humanTaskRepository = humanTaskRepository;
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

        public void CreateTask(int taskId)
        {
            var task = humanTaskRepository.GetById(taskId);
            int projectId = task.ProjectId;
            var clients = SignalRClients.Connections.Where(it => it.ProjectId == projectId);
            var context = GlobalHost.ConnectionManager.GetHubContext<TaskHub>();
            foreach (var clientConnection in clients)
            {
                context.Clients[clientConnection.ConnectionId].TaskCreated(taskId,task.AssigneeId);
            }
        }
    }
}