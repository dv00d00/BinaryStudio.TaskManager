using System.Collections.Generic;
using BinaryStudio.TaskManager.Logic.Domain;
using BinaryStudio.TaskManager.Web.SignalR;
using SignalR;
using SignalR.Hubs;
namespace BinaryStudio.TaskManager.Logic.Core
{
    using System;

    using BinaryStudio.TaskManager.Extensions;
    using BinaryStudio.TaskManager.Extensions.Extentions;
    using BinaryStudio.TaskManager.Extensions.Models;

    public class Notifier : INotifier
    {
        private readonly IHumanTaskRepository humanTaskRepository;
        private readonly INewsRepository newsRepository;
        private readonly IGlobalHost globalHost;
        private readonly IConnectionProvider connectionProvider;
        private readonly IProjectRepository projectRepository;
        private readonly IUserProcessor userProcessor;
        private readonly IUserRepository userRepository;
        private readonly IStringExtensions stringExtensions;

        public Notifier(IHumanTaskRepository humanTaskRepository, IGlobalHost globalHost, IConnectionProvider connectionProvider,
            INewsRepository newsRepository,IProjectRepository projectRepository, IUserProcessor userProcessor, 
            IUserRepository userRepository, IStringExtensions stringExtensions)
        {
            this.humanTaskRepository = humanTaskRepository;
            this.globalHost = globalHost;
            this.connectionProvider = connectionProvider;
            this.newsRepository = newsRepository;
            this.projectRepository = projectRepository;
            this.userProcessor = userProcessor;
            this.userRepository = userRepository;
            this.stringExtensions = stringExtensions;
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
            var taskToSend = new LandingTaskModel
            {
                Id = task.Id,
                Description = stringExtensions.Truncate(task.Description, 70),
                Name = stringExtensions.Truncate(task.Name, 70),
                Priority = task.Priority,
                Created = task.Created,
                Creator = userRepository.GetById(task.CreatorId.GetValueOrDefault()).UserName,
                AssigneeId = task.AssigneeId,
                Assignee = task.AssigneeId == null ? null : userRepository.GetById(task.AssigneeId.GetValueOrDefault()).UserName,
                AssigneePhoto = task.AssigneeId == null ? false : userRepository.GetById(task.AssigneeId.GetValueOrDefault()).ImageData != null,
                Finished = task.Finished
            };
            var clients = this.connectionProvider.GetProjectConnections(projectId);
            var context = GlobalHost.ConnectionManager.GetHubContext<TaskHub>();

            foreach (var clientConnection in clients)
            {
                    context.Clients[clientConnection.ConnectionId].TaskCreated(taskToSend);
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

        public void BroadcastNewsToDesktopClient(HumanTaskHistory taskHistory)
        {
            var connects = this.connectionProvider.GetClientConnectionsForUser(taskHistory.NewAssigneeId.Value);
            var context = GlobalHost.ConnectionManager.GetHubContext<TaskHub>();
            string message = taskHistory.Action;
            string userName = this.userProcessor.GetUser(taskHistory.UserId).UserName;
            switch (taskHistory.Action)
            {
                case ChangeHistoryTypes.Create :
                    {
                        message = userName +
                                  " created task '" + taskHistory.NewName + "' for you";
                        break;
                    }
                case ChangeHistoryTypes.Change :
                    {
                        message = userName + " changed your task '" + taskHistory.NewName + "'";
                        break;
                    }
                case ChangeHistoryTypes.Close :
                    {
                        message = userName + " closed your task '" + taskHistory.NewName + "'";
                        break;
                    }
                case ChangeHistoryTypes.Move:
                    {
                        message = userName + " assigned task '" + taskHistory.NewName + "' to you";
                        break;
                    }             
            }
           
            foreach (var clientConnection in connects)
            {
                context.Clients[clientConnection.ConnectionId].ReciveMessageOnClient(message,taskHistory.Task.ProjectId);
            }
        }

        public bool SendReminderToDesktopClient(int userId, string message,int projectId)
        {
            var connects = new List<ClientConnection>(this.connectionProvider.GetClientConnectionsForUser(userId));
            var context = GlobalHost.ConnectionManager.GetHubContext<TaskHub>();
            if (connects.Count > 0)
            {
                foreach (var clientConnection in connects)
                {
                    context.Clients[clientConnection.ConnectionId].ReciveMessageOnClient(message,projectId);
                }
                return true;
            }
            return false;
        }

        public void BroadcastNews(News news)
        {
            //BroadcastNewsToDesktopClient(news);
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