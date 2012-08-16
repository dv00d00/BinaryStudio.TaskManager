using BinaryStudio.TaskManager.Logic.Domain;
using BinaryStudio.TaskManager.Web.SignalR;
using SignalR;
using SignalR.Hubs;
namespace BinaryStudio.TaskManager.Logic.Core
{
    using System;
    public class UserToAssign
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public bool Photo { get; set; }
    }
    public class Notifier : INotifier
    {
        private readonly IHumanTaskRepository humanTaskRepository;
        private readonly INewsRepository newsRepository;
        private readonly IGlobalHost globalHost;
        private readonly IConnectionProvider connectionProvider;
        private readonly IProjectRepository projectRepository;
        private readonly IUserProcessor userProcessor;
        private readonly IUserRepository userRepository;

        public Notifier(IHumanTaskRepository humanTaskRepository, IGlobalHost globalHost, IConnectionProvider connectionProvider, INewsRepository newsRepository,IProjectRepository projectRepository, IUserProcessor userProcessor, IUserRepository userRepository)
        {
            this.humanTaskRepository = humanTaskRepository;
            this.globalHost = globalHost;
            this.connectionProvider = connectionProvider;
            this.newsRepository = newsRepository;
            this.projectRepository = projectRepository;
            this.userProcessor = userProcessor;
            this.userRepository = userRepository;
        }

        public void MoveTask(HumanTask task, int moveToId)
        {
            var model = new UserToAssign
            {
                Id = moveToId == -1 ? (int?)null : moveToId,
                Name = moveToId != -1 ? userRepository.GetById(moveToId).UserName : "",
                Photo = moveToId != -1 ? userRepository.GetById(moveToId).ImageData != null : false
            };
            // send notificantion
            //moveToId = moveToId == -1 ? 0 : moveToId;
            var clients = this.connectionProvider.GetProjectConnections(task.ProjectId);

            var context = GlobalHost.ConnectionManager.GetHubContext<TaskHub>();
            foreach (var clientConnection in clients)
            {
                context.Clients[clientConnection.ConnectionId].TaskMoved(task.Id, model);
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
        }

        public void SetCountOfNews(string userName)
        {
            int count = newsRepository.GetUnreadNewsCountForUserByName(userName);
            var clients = this.connectionProvider.GetConnetionsForUser(userName);
            var context = GlobalHost.ConnectionManager.GetHubContext<TaskHub>();

            foreach (var clientConnection in clients)
            {
                context.Clients[clientConnection.ConnectionId].SetUnreadNewsesCount(count);
            }
        }

        public void SetCountOfNews(int userId)
        {
            var user = userProcessor.GetUser(userId);
            SetCountOfNews(user.UserName);
        }

        public void BroadcastNewsToDesktopClient(News news)
        {
            var connects = this.connectionProvider.GetClientConnectionsForUser(news.UserId);
            var context = GlobalHost.ConnectionManager.GetHubContext<TaskHub>();
            string message = news.HumanTaskHistory.Action;
            foreach (var clientConnection in connects)
            {
                context.Clients[clientConnection.ConnectionId].ReciveMessageOnClient(message);
            }
            
        }

        public void BroadcastNews(News news)
        {
            BroadcastNewsToDesktopClient(news);
            this.SetCountOfNews(news.UserId);
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