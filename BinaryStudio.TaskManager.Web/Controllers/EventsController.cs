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
                eventsViewModels.Add(new EventViewModel
                                         {
                                             ProjectId = newse.HumanTaskHistory.Task.ProjectId,
                                             ProjectName = newse.HumanTaskHistory.Task.Project.Name,
                                             TaskId = newse.HumanTaskHistory.TaskId,
                                             TaskName = newse.HumanTaskHistory.NewName,
                                             WhoChangeUserName = userProcessor.GetUser(newse.HumanTaskHistory.UserId).UserName,
                                             WhoChangeUserId = newse.HumanTaskHistory.UserId,
                                             Action = newse.HumanTaskHistory.Action,
                                             NewsId = newse.Id,
                                             TimeAgo = dateTimeDifference.Hours > 24
                                             ? dateTimeDifference.Days.ToString() + " days ago "
                                             :"about " + dateTimeDifference.Hours.ToString() + " hours ago ",
                                             Details = newse.HumanTaskHistory.NewDescription == null ? "" : newse.HumanTaskHistory.NewDescription.Substring(0, 3) + "..."
 
                                         });
            }
            
            return View(eventsViewModels);
        }

    }
}
