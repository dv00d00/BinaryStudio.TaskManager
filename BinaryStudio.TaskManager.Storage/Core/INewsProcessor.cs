using System.Collections.Generic;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{
    public interface INewsProcessor
    {
        void CreateNewsForUsersInProject(HumanTaskHistory taskHistory, int projectId);

        void AddNews(HumanTaskHistory humanTaskHistory, User user);

        void AddNews(News news);
    }

    public class NewsProcessor : INewsProcessor
    {
        private readonly INewsRepository newsRepository;
        private readonly INotifier notifier;
        private readonly IProjectProcessor projectProcessor;

        public NewsProcessor(INotifier notifier, INewsRepository newsRepository, IProjectProcessor projectProcessor)
        {
            this.notifier = notifier;
            this.newsRepository = newsRepository;
            this.projectProcessor = projectProcessor;
        }

        public void CreateNewsForUsersInProject(HumanTaskHistory taskHistory, int projectId)
        {
            var projectUsers = new List<User>(this.projectProcessor.GetUsersAndCreatorInProject(projectId));
            foreach (var projectUser in projectUsers)
            {
                this.AddNews(taskHistory, projectUser);
            }
        }

        public void AddNews(HumanTaskHistory taskHistory, User user)
        {
            var news = new News
            {
                HumanTaskHistory = taskHistory,
                IsRead = false,
                User = user,
                UserId = user.Id,
                HumanTaskHistoryId = taskHistory.Id,
            };
            this.AddNews(news);
        }

        public void AddNews(News news)
        {
            this.newsRepository.AddNews(news);
            this.notifier.BroadcastNews(news);
        }
    }
}
