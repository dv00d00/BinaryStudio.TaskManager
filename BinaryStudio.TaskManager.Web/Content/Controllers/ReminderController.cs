using System.Web.Mvc;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Web.Controllers
{
    using System.Collections.Generic;

    using BinaryStudio.TaskManager.Logic.Core;

    public class ReminderController : Controller
    {
        private readonly IReminderRepository reminderRepository;
        private readonly IEmployeeRepository employeeRepository;

        private readonly ITaskProcessor taskProcessor;

        public ReminderController(IReminderRepository reminderRepository, 
            IEmployeeRepository employeeRepository, ITaskProcessor taskProcessor)
        {
            this.reminderRepository = reminderRepository;
            this.employeeRepository = employeeRepository;
            this.taskProcessor = taskProcessor;
        }

        //
        // GET: /Reminder/

        public ViewResult Index()
        {
              return View(reminderRepository.GetAll());
        }

        //
        // GET: /Reminder/Details/5

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
            ViewBag.PossibleEmployees = employeeRepository.GetAll();
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
            ViewBag.PossibleEmployees = employeeRepository.GetAll();
            
            return View(reminder);
            }
        
        //
        // GET: /Reminder/Edit/5
 
        public ActionResult Edit(int id)
        {
            Reminder reminder = reminderRepository.GetById(id);
            ViewBag.PossibleTasks = taskProcessor.GetAllTasks();
            ViewBag.PossibleEmployees = employeeRepository.GetAll();
            return View(reminder);
        }

        //
        // POST: /Reminder/Edit/5

        [HttpPost]
        public ActionResult Edit(Reminder reminder)
        {
            if (ModelState.IsValid)
            {
                reminderRepository.Update(reminder);
                return RedirectToAction("Index");
            }
            ViewBag.PossibleTasks = taskProcessor.GetAllTasks();
            ViewBag.PossibleEmployees = employeeRepository.GetAll();
            return View(reminder);
        }

        //
        // GET: /Reminder/Delete/5
 
        public ActionResult Delete(int id)
        {
            Reminder reminder = reminderRepository.GetById(id);
            return View(reminder);
        }

        //
        // POST: /Reminder/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Reminder reminder = reminderRepository.GetById(id); 
            reminderRepository.Delete(reminder);
            return RedirectToAction("Index");
        }
    }
}