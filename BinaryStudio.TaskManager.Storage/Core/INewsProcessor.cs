using System.Collections.Generic;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{
    public interface INewsProcessor
    {
        void CreateNewsForUsersInProject(HumanTaskHistory taskHistory, int projectId);
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
                var news = new News
                {
                    HumanTaskHistory = taskHistory,
                    IsRead = false,
                    User = projectUser,
                    UserId = projectUser.Id,
                    HumanTaskHistoryId = taskHistory.Id,
                };

                newsRepository.AddNews(news);
                notifier.SetCountOfNewses(projectUser.UserName);
            }
        }
    }
}
