using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BinaryStudio.TaskManager.Logic.Domain;
using BinaryStudio.TaskManager.Web.Models;
using BinaryStudio.TaskManager.Logic.Core;

namespace BinaryStudio.TaskManager.Web.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly IUserProcessor userProcessor;
        private readonly INewsRepository newsRepository;
        private readonly INotifier notifier;
        private readonly IProjectRepository projectRepository;
        public EventsController(IUserProcessor userProcessor, INewsRepository newsRepository, INotifier notifier, IProjectRepository projectRepository)
        {
            this.userProcessor = userProcessor;
            this.newsRepository = newsRepository;
            this.notifier = notifier;
            this.projectRepository = projectRepository;
        }

        //
        // GET: /Events/

        public ActionResult MyEvents()
        {
            var eventsViewModels = new List<EventViewModel>();
            var user = userProcessor.GetUserByName(User.Identity.Name).Id; 
            List<News> news = new List<News>(newsRepository.GetAllNewsForUser(user).OrderByDescending(x=> x.HumanTaskHistory.ChangeDateTime));
           
            foreach (var newse in news)
            {
                eventsViewModels.Add(CreateEventViewModel(newse));
            }
            var listEvents = new ListEventViewModel();
            listEvents.Events = eventsViewModels;
            listEvents.Projects = new List<ProjectDataForEventsViewModel>();
            listEvents.Projects.AddRange(userProcessor.GetUserByName(User.Identity.Name).UserProjects.Select(
                x => new ProjectDataForEventsViewModel{ProjectId = x.Id,ProjectName = x.Name}));
            listEvents.Projects.AddRange(projectRepository.GetAllProjectsForTheirCreator(user).Select(
                x => new ProjectDataForEventsViewModel { ProjectId = x.Id, ProjectName = x.Name }));

            return View(listEvents);
        }


        public EventViewModel CreateEventViewModel(News news)
        {
            return new EventViewModel
                       {
                           ProjectId = news.HumanTaskHistory.Task.ProjectId,
                           ProjectName = news.HumanTaskHistory.Task.Project.Name,
                           TaskId = news.HumanTaskHistory.TaskId,
                           TaskName = news.HumanTaskHistory.NewName,
                           WhoChangeUserName = userProcessor.GetUser(news.HumanTaskHistory.UserId).UserName,
                           WhoChangeUserId = news.HumanTaskHistory.UserId,
                           Action = news.HumanTaskHistory.Action,
                           NewsId = news.Id,
                           TimeAgo = TakeTimeAgo(news.HumanTaskHistory.ChangeDateTime),
                           Details =
                               news.HumanTaskHistory.NewDescription == null
                                   ? ""
                                   : news.HumanTaskHistory.NewDescription.Length>26 ? news.HumanTaskHistory.NewDescription.Substring(0, 25) + "..."
                                   :"",
                          IsRead = news.IsRead,
                          WhoAssigneUserId = news.HumanTaskHistory.NewAssigneeId,
                          WhoAssigneUserName = news.HumanTaskHistory.NewAssigneeId.HasValue ? userProcessor.GetUser(news.HumanTaskHistory.NewAssigneeId.Value).UserName : "",
                          ContainerClassName = news.IsRead ? "container evnt_read" : "container evnt_unread",
                          TaskLinkDetails = "/Project/Details/"+ news.HumanTaskHistory.TaskId,
                           WhoAssigneLinkDetails = "/Project/UserDetails?userId=" + news.HumanTaskHistory.NewAssigneeId,
                           WhoChangeLinkDetails = "/Project/UserDetails?userId=" + news.HumanTaskHistory.UserId,
                           IsMove = news.HumanTaskHistory.Action == ChangeHistoryTypes.Move ? true : false,
                           IsAssigne = news.HumanTaskHistory.NewAssigneeId.HasValue
                          };
        }

        public string TakeTimeAgo(DateTime time)
        {
            var dateTimeDifference = DateTime.Now.Subtract(time);
            if(dateTimeDifference.TotalMinutes < 1)
            {
                return "just now";
            }
            if (dateTimeDifference.TotalHours < 1)
            {
                return dateTimeDifference.Minutes.ToString() + " minutes ago";
            }
            if (dateTimeDifference.TotalHours < 24)
            {
                return dateTimeDifference.Hours.ToString() + " hours ago";
            }
            if (dateTimeDifference.TotalHours > 24 && dateTimeDifference.TotalHours < 48)
            {
                return "1 day ago";
            }
            if (dateTimeDifference.TotalHours > 48)
            {
                return Math.Floor(dateTimeDifference.TotalDays).ToString() + " days ago";
            }
            return time.ToString();
        }

        public void MarkAsRead(int newsId)
        {
            newsRepository.MarkAsRead(newsId);
            News news = this.newsRepository.GetNewsById(newsId);
            this.notifier.SetCountOfNewses(news.User.UserName);
        }

        [HttpPost]
        public ActionResult GetNewsCount()
        {
            int count = newsRepository.GetNewsCount(userProcessor.GetUserByName(User.Identity.Name).Id);
            return Json(count);
        }

        public void MarkAllUnreadNewsAsRead()
        {
            var user = userProcessor.GetUserByName(User.Identity.Name);
            newsRepository.MarkAllUnreadNewsForUser(user.Id);
            this.notifier.SetCountOfNewses(user.UserName);
        }

        // type == 1, all news about all tasks in my projects
        // type == 2, news about only me
        // type > 2, news for some project
       
        public ActionResult GetNews(ListEventViewModel eventsViewModels,  int type,int projectId=-1)
        {
            //var eventsViewModels = new List<EventViewModel>();
            eventsViewModels.Events=new List<EventViewModel>();
            var user = userProcessor.GetUserByName(User.Identity.Name).Id;
            List<News> news = new List<News>(newsRepository.GetAllNewsForUser(user).OrderByDescending(x => x.HumanTaskHistory.ChangeDateTime));
            if (type == 2)
            {
                news = news.Where(
                        x =>
                        x.HumanTaskHistory.NewAssigneeId == user ||
                        x.HumanTaskHistory.UserId == user ).ToList();
            }
            if(type > 2)
            {
                news = news.Where(x => x.HumanTaskHistory.Task.ProjectId == projectId).ToList();
            }

            foreach (var newse in news)
            {
                eventsViewModels.Events.Add(CreateEventViewModel(newse));
            }

            eventsViewModels.Projects = new List<ProjectDataForEventsViewModel>();
            eventsViewModels.Projects.AddRange(userProcessor.GetUserByName(User.Identity.Name).UserProjects.Select(
                x => new ProjectDataForEventsViewModel { ProjectId = x.Id, ProjectName = x.Name }));
            eventsViewModels.Projects.AddRange(projectRepository.GetAllProjectsForTheirCreator(user).Select(
                x => new ProjectDataForEventsViewModel { ProjectId = x.Id, ProjectName = x.Name }));

            //eventsViewModels.Projects = new List<Project>(userProcessor.GetUserByName(User.Identity.Name).UserProjects);
            return Json(eventsViewModels);
            
        }


    }
}
