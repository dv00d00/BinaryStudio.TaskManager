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

        public ReminderController(
            IReminderRepository reminderRepository,
            IUserRepository userRepository,
            ITaskProcessor taskProcessor)
        {
            this.reminderRepository = reminderRepository;
            this.userRepository = userRepository;
            this.taskProcessor = taskProcessor;
        }

        //
        // GET: /Reminder/

        public ViewResult Index()
        {
            return View(reminderRepository.GetAll());
        }

        //
        // GET: /Reminder/DetailsEmployee/5

        public ViewResult Details(int id)
        {
            Reminder reminder = reminderRepository.GetById(id);
            return View(reminder);
        }

        //
        // GET: /Reminder/Create

        public ActionResult Create()
        {
            ViewBag.PossibleTasks = taskProcessor.GetAllTasks();
            //ViewBag.PossibleEmployees = employeeRepository.GetAll();
            return View();
        }

        //
        // POST: /Reminder/Create

        [HttpPost]
        public ActionResult Create(Reminder reminder)
        {
            if (ModelState.IsValid)
            {
                reminderRepository.Add(reminder);
                return RedirectToAction("Index");
            }
            ViewBag.PossibleTasks = taskProcessor.GetAllTasks();
            //ViewBag.PossibleEmployees = employeeRepository.GetAll();

            return View(reminder);
        }

        //
        // GET: /Reminder/EditEmployee/5

        public ActionResult Edit(int id)
        {
            Reminder reminder = reminderRepository.GetById(id);
            ViewBag.PossibleTasks = taskProcessor.GetAllTasks();
            //ViewBag.PossibleEmployees = employeeRepository.GetAll();
            return View(reminder);
        }

        //
        // POST: /Reminder/EditEmployee/5

        [HttpPost]
        public ActionResult Edit(Reminder reminder)
        {
            if (ModelState.IsValid)
            {
                reminderRepository.Update(reminder);
                return RedirectToAction("Index");
            }
            ViewBag.PossibleTasks = taskProcessor.GetAllTasks();
            //ViewBag.PossibleEmployees = employeeRepository.GetAll();
            return View(reminder);
        }

        //
        // GET: /Reminder/DeleteEmployee/5

        public ActionResult Delete(int id)
        {
            Reminder reminder = reminderRepository.GetById(id);
            return View(reminder);
        }

        //
        // POST: /Reminder/DeleteEmployee/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Reminder reminder = reminderRepository.GetById(id);
            reminderRepository.Delete(reminder);
            return RedirectToAction("Index");
        }
    }
}