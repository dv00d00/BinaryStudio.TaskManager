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
        public EventsController(IUserProcessor userProcessor )
        {
            this.userProcessor = userProcessor;
        }

        //
        // GET: /Events/

        public ActionResult MyEvents()
        {
            var eventsViewModels = new List<EventViewModel>();
            List<News> news =new List<News>( userProcessor.GetAllNewsForUser(userProcessor.GetUserByName(User.Identity.Name).Id));
            foreach (var newse in news)
            {
                eventsViewModels.Add(new EventViewModel
                                         {
                                             ProjectId = newse.HumanTaskHistory.Task.ProjectId,
                                             ProjectName = newse.HumanTaskHistory.Task.Project.Name,
                                             TaskId = newse.HumanTaskHistory.TaskId,
                                             TaskName = newse.HumanTaskHistory.NewName,
                                             WhoChangeUserName = userProcessor.GetUser(newse.HumanTaskHistory.UserId).UserName,
                                             WhoChangeUserId = newse.HumanTaskHistory.UserId,
                                             Action = newse.HumanTaskHistory.Action,
                                             
                                         });
            }
            return View(eventsViewModels);
        }

    }
}
