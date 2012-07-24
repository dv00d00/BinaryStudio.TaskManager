using System;

namespace BinaryStudio.TaskManager.Web.Controllers
{
    using System.Web.Mvc;

    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;
    [Authorize]
    public class ReminderController : Controller
    {
        private readonly IReminderRepository reminderRepository;
        private readonly ITaskProcessor taskProcessor;
        private readonly IUserRepository userRepository;

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
            ITaskProcessor taskProcessor)
        {
            this.reminderRepository = reminderRepository;
            this.userRepository = userRepository;
            this.taskProcessor = taskProcessor;
        }
        
        public ViewResult MyReminders()
        {
            return this.View(this.reminderRepository.GetAllRemindersForUser(userRepository.GetByName(User.Identity.Name).Id));
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
            return this.RedirectToAction("Index");
        }
    }
}