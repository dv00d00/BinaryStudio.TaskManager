using System;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using BinaryStudio.TaskManager.Logic.Domain;
using BinaryStudio.TaskManager.Web.SignalR;
using SignalR;
using SignalR.Hubs;

namespace BinaryStudio.TaskManager.Logic.Core
{
    public class Notifier : INotifier
    {
        private readonly IHumanTaskRepository humanTaskRepository;
        private readonly INewsRepository newsRepository;
        private readonly IGlobalHost globalHost;
        private readonly IConnectionProvider connectionProvider;
        private readonly IProjectRepository projectRepository;
        private readonly IUserProcessor userProcessor;

        public Notifier(IHumanTaskRepository humanTaskRepository, IGlobalHost globalHost, IConnectionProvider connectionProvider, INewsRepository newsRepository,IProjectRepository projectRepository, IUserProcessor userProcessor)
        {
            this.humanTaskRepository = humanTaskRepository;
            this.globalHost = globalHost;
            this.connectionProvider = connectionProvider;
            this.newsRepository = newsRepository;
            this.projectRepository = projectRepository;
            this.userProcessor = userProcessor;
        }

        public void MoveTask(int taskId, int moveToId)
        {
            // send notificantion
            moveToId = moveToId == -1 ? 0 : moveToId;
            var task = humanTaskRepository.GetById(taskId);
            var clients = this.connectionProvider.GetProjectConnections(task.ProjectId);

            var context = GlobalHost.ConnectionManager.GetHubContext<TaskHub>();
            foreach (var clientConnection in clients)
            {
                context.Clients[clientConnection.ConnectionId].TaskMoved(taskId, moveToId);
            }

            string wpfMessage = "Task " + task.Name + " assigned to " + (moveToId == 0 ? " unassigned" : this.userProcessor.GetUser(moveToId).UserName);
            var wpfClients = this.connectionProvider.GetWPFConnectionsForProject(task.ProjectId);

            foreach (var clientConnection in wpfClients)
            {
                context.Clients[clientConnection.ConnectionId].ReciveMessageOnClient(wpfMessage);
            }

        }

        public void CreateTask(int taskId)
        {
            var task = humanTaskRepository.GetById(taskId);
            int projectId = task.ProjectId;
            var project = projectRepository.GetById(projectId);
            int assignedId = task.AssigneeId ?? 0; 
            var clients = this.connectionProvider.GetProjectConnections(projectId);
            var context = GlobalHost.ConnectionManager.GetHubContext<TaskHub>();

            foreach (var clientConnection in clients)
            {
                    context.Clients[clientConnection.ConnectionId].TaskCreated(taskId, assignedId);
            }

            string wpfMessage = "Task " + task.Name + " added to project " + project.Name;
            var wpfClients = this.connectionProvider.GetWPFConnectionsForProject(projectId);

            foreach (var clientConnection in wpfClients)
            {
                context.Clients[clientConnection.ConnectionId].ReciveMessageOnClient(wpfMessage);
            }
        }

        public void SetCountOfNewses(string userName)
        {
            int count = newsRepository.GetUnreadNewsCountForUserByName(userName);
            var clients = this.connectionProvider.GetConnetionsForUser(userName);
            var context = GlobalHost.ConnectionManager.GetHubContext<TaskHub>();

            foreach (var clientConnection in clients)
            {
                context.Clients[clientConnection.ConnectionId].SetUnreadNewsesCount(count);
            }
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