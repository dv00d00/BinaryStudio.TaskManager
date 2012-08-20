using System;
using System.Web.Configuration;
using System.Configuration;
namespace BinaryStudio.TaskManager.Web.Controllers
{
    using System.Web.Mvc;

    using Logic.Core;
    using Logic.Domain;
    
    public class ReminderController : Controller
    {
        private readonly IReminderRepository reminderRepository;
        private readonly ITaskProcessor taskProcessor;
        private readonly IUserRepository userRepository;
        private readonly IReminderProcessor reminderProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReminderController"/> class.
        /// </summary>
        /// <param name="reminderRepository">
        /// The reminder repository.
        /// </param>
        /// <param name="userRepository">
        /// The user repository.
        /// </param>
        /// <param name="taskProcessor">
        /// The task processor.
        /// </param>
        public ReminderController(
            IReminderRepository reminderRepository,
            IUserRepository userRepository,
            ITaskProcessor taskProcessor,
            IReminderProcessor reminderProcessor)
        {
            this.reminderRepository = reminderRepository;
            this.userRepository = userRepository;
            this.taskProcessor = taskProcessor;
            this.reminderProcessor = reminderProcessor;
        }
        [Authorize]
        public ViewResult MyReminders()
        {
            return
                this.View(this.reminderProcessor.GetRemindersForUser((userRepository.GetByName(User.Identity.Name).Id)));//  GetAllRemindersForUser(userRepository.GetByName(User.Identity.Name).Id));
        }
        
        public ViewResult Details(int id)
        {
            var reminder = this.reminderRepository.GetById(id);
            return this.View(reminder);
        }

        public ActionResult Create()
        {
            ViewBag.PossibleTasks = this.taskProcessor.GetTasksList(userRepository.GetByName(User.Identity.Name).Id);
            var reminder = new Reminder();
            reminder.ReminderDate = DateTime.Now;
            return this.View(reminder);
        }
        
        [HttpPost]
        public ActionResult Create(Reminder reminder)
        {
            if (ModelState.IsValid)
            {
                reminder.UserId = userRepository.GetByName(User.Identity.Name).Id;
                this.reminderRepository.Add(reminder);
                return this.RedirectToAction("MyReminders");
            }
            ViewBag.PossibleTasks = this.taskProcessor.GetAllTasks();
            ViewBag.PossibleEmployees = this.userRepository.GetAll();

            return this.View(reminder);
        }

        
        [HttpPost]
        public JsonResult GetUsers (string name, string applicationKey)
        {
            Configuration myWebConfig = WebConfigurationManager.OpenWebConfiguration(null);
            KeyValueConfigurationElement customSetting = myWebConfig.AppSettings.Settings["IntegrationKey"];
            if(customSetting!=null)
            {
                if(customSetting.Value==applicationKey)
                {
                    var user = userRepository.GetByName(name);
                    string userName = user != null ? user.UserName :  "";
                    int userId = user != null ? user.Id : -1;
                    return Json(new object[] {userName,userId});
                }
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult SendNotification(int userId,string content,DateTime date)
        {
            var reminder = new Reminder
                               {
                                   UserId = userId,
                                   Content = content,
                                   ReminderDate = date,
                                   IsSend = false,
                                   WasDelivered = false,
                                   TaskId = null,
                               };
            try
            {
                reminderProcessor.AddReminder(reminder);
            }
            catch(Exception e)
            {
                return Json(false);
            }
            return Json(true);
        }

        public ActionResult Edit(int id)
        {
            var reminder = this.reminderRepository.GetById(id);
            ViewBag.PossibleTasks = this.taskProcessor.GetAllTasks();
            ViewBag.PossibleEmployees = this.userRepository.GetAll();
            return this.View(reminder);
        }
        
        [HttpPost]
        public ActionResult Edit(Reminder reminder)
        {
            if (ModelState.IsValid)
            {
                reminder.UserId = userRepository.GetByName(User.Identity.Name).Id;
                this.reminderRepository.Update(reminder);
                return this.RedirectToAction("MyReminders");
            }
            ViewBag.PossibleTasks = this.taskProcessor.GetAllTasks();
            ViewBag.PossibleEmployees = this.userRepository.GetAll();
            return this.View(reminder);
        }

        public ActionResult Delete(int id)
        {
            var reminder = this.reminderRepository.GetById(id);
            return this.View(reminder);
        }
        
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Reminder reminder = this.reminderRepository.GetById(id);
            this.reminderRepository.Delete(reminder);
            return this.RedirectToAction("MyReminders");
        }
        [Authorize]
        public ActionResult IsUserHasReminders()
        {
            int userId = userRepository.GetByName(User.Identity.Name).Id;
            return Json(reminderProcessor.IsUserHaveReminders(userId));
        }
    }
}