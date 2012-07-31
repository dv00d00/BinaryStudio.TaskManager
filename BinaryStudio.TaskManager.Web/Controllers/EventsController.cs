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
        public EventsController(IUserProcessor userProcessor, INewsRepository newsRepository )
        {
            this.userProcessor = userProcessor;
            this.newsRepository = newsRepository;
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
                var dateTimeDifference = DateTime.Now.Subtract(newse.HumanTaskHistory.ChangeDateTime);
                eventsViewModels.Add(CreateEventViewModel(newse));
            }
            
            return View(eventsViewModels);
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
                          IsRead = news.IsRead
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
                return dateTimeDifference.TotalDays.ToString() + " days ago";
            }
            return time.ToString();
        }
    }
}
