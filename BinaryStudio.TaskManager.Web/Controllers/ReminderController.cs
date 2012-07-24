namespace BinaryStudio.TaskManager.Web.Controllers
{
    using System.Web.Mvc;

    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;

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
        
        public ViewResult Index()
        {
            return this.View(this.reminderRepository.GetAll());
        }
        
        public ViewResult Details(int id)
        {
            var reminder = this.reminderRepository.GetById(id);
            return this.View(reminder);
        }

        public ActionResult Create()
        {
            ViewBag.PossibleTasks = this.taskProcessor.GetAllTasks();
            ViewBag.PossibleEmployees = this.userRepository.GetAll();
            return this.View();
        }
        
        [HttpPost]
        public ActionResult Create(Reminder reminder)
        {
            if (ModelState.IsValid)
            {
                this.reminderRepository.Add(reminder);
                return this.RedirectToAction("Index");
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
                this.reminderRepository.Update(reminder);
                return this.RedirectToAction("Index");
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