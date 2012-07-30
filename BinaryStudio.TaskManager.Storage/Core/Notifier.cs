using System.Linq;
using System.Runtime.Remoting.Contexts;
using BinaryStudio.TaskManager.Logic.Core.SignalR;
using BinaryStudio.TaskManager.Web.SignalR;
using SignalR;
using SignalR.Hubs;

namespace BinaryStudio.TaskManager.Logic.Core
{
    using System.Collections.Generic;

    public class Notifier : INotifier
    {
        private readonly IHumanTaskRepository humanTaskRepository;
        private readonly IGlobalHost globalHost;
        private readonly IConnectionProvider connectionProvider;

        public Notifier(IHumanTaskRepository humanTaskRepository, IGlobalHost globalHost, IConnectionProvider connectionProvider)
        {
            this.humanTaskRepository = humanTaskRepository;
            this.globalHost = globalHost;
            this.connectionProvider = connectionProvider;
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

            //var context = this.globalHost.GetContext<TaskHub>();
            var context = GlobalHost.ConnectionManager.GetHubContext<TaskHub>();
            context.Clients.TaskMoved(taskId, moveToId);
        }

        public void CreateTask(int taskId)
        {
            var task = humanTaskRepository.GetById(taskId);

            int projectId = task.ProjectId;
            int assignedId = task.AssigneeId ?? 0; 

            var clients = this.connectionProvider.GetProjectConnections(projectId);
            //var context = this.globalHost.GetContext<TaskHub>();
            var context = GlobalHost.ConnectionManager.GetHubContext<TaskHub>();

            foreach (var clientConnection in clients)
            {
                context.Clients[clientConnection.ConnectionId].TaskCreated(taskId, assignedId);
            }
        }
    }

    public interface IConnectionProvider
    {
        IEnumerable<ClientConnection> ActiveConnections { get; }

        IEnumerable<ClientConnection> GetProjectConnections(int projectId);
    }

    public class ConnectionProvider : IConnectionProvider
    {
        public IEnumerable<ClientConnection> ActiveConnections
        {
            get
            {
                return SignalRClients.Connections;
            }
        }

        public IEnumerable<ClientConnection> GetProjectConnections(int projectId)
        {
            return SignalRClients.Connections.Where(it => it.ProjectId == projectId);
        }
    }

    public interface IGlobalHost
    {
        dynamic GetContext<TContext>() where TContext : IHub;
    }

    public class GlobalHostImpl : IGlobalHost
    {
        public dynamic GetContext<TContext>() where TContext : IHub
        {
            return GlobalHost.ConnectionManager.GetHubContext<TContext>();
        }
    }
}