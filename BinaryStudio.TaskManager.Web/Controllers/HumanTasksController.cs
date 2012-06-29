using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Web.Models;

    /// <summary>
    /// Provides access to human task entities
    /// </summary>
    public class HumanTasksController : Controller
    {
        private readonly IHumanTaskRepository humanTaskRepository;

        private readonly ITaskProcessor taskProcessor;

        private readonly IEmployeeRepository employeeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="HumanTasksController"/> class.
        /// </summary>
        /// <param name="humanTaskRepository">The human task repository.</param>
        /// <param name="taskProcessor">The task processor.</param>
        public HumanTasksController(IHumanTaskRepository humanTaskRepository, ITaskProcessor taskProcessor, IEmployeeRepository employeeRepository)
        {
            this.humanTaskRepository = humanTaskRepository;
            this.taskProcessor = taskProcessor;
            this.employeeRepository = employeeRepository;
        }

        //
        // GET: /HumanTasks/

        public ViewResult Index()
        {
            var humanTasks = this.humanTaskRepository.GetAll();

            return View(humanTasks);
        }

        //
        // GET: /HumanTasks/Details/5

        public ViewResult Details(int id)
        {
            HumanTask humantask = this.humanTaskRepository.GetById(id);
            return View(humantask);
        }

        //
        // GET: /HumanTasks/Create

        public ActionResult Create()
        {
            ViewBag.PossibleCreators = new List<Employee>();
            ViewBag.PossibleAssignees = new List<Employee>();
            return View();
        }

        //
        // POST: /HumanTasks/Create

        [HttpPost]
        public ActionResult Create(HumanTask humanTask)
        {
            if (ModelState.IsValid)
            {
                this.humanTaskRepository.Add(humanTask);
                return RedirectToAction("Index");
            }

            ViewBag.PossibleCreators = new List<Employee>();
            ViewBag.PossibleAssignees = new List<Employee>();
            return View(humanTask);
        }

        //
        // GET: /HumanTasks/Edit/5

        public ActionResult Edit(int id)
        {
            HumanTask humantask = this.humanTaskRepository.GetById(id);
            ViewBag.PossibleCreators = new List<Employee>();
            ViewBag.PossibleAssignees = new List<Employee>();
            return View(humantask);
        }

        //
        // POST: /HumanTasks/Edit/5

        [HttpPost]
        public ActionResult Edit(HumanTask humanTask)
        {
            if (ModelState.IsValid)
            {
                this.humanTaskRepository.Update(humanTask);
                return RedirectToAction("Index");
            }

            ViewBag.PossibleCreators = new List<Employee>();
            ViewBag.PossibleAssignees = new List<Employee>();
            return View(humanTask);
        }

        //
        // GET: /HumanTasks/Delete/5

        public ActionResult Delete(int id)
        {
            HumanTask humantask = this.humanTaskRepository.GetById(id);
            return View(humantask);
        }

        //
        // POST: /HumanTasks/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            this.humanTaskRepository.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult ManagerDetails(int id)
        {
            var model = new ManagerTasksViewModel();
            model.Manager = employeeRepository.GetById(id);
            model.Tasks = taskProcessor.GetTasksList(id).ToList();
            return View(model);
        }

        public ActionResult AllManagersWithTasks()
        {
            var model = CreateManagersViewModel();

            return View(model);
        }

        [HttpPost]
        public void MoveTask(int oldEmployeeId, int newEmployeeId, int taskId)
        {
            this.taskProcessor.MoveTask(oldEmployeeId, taskId);
        }

        private ManagersViewModel CreateManagersViewModel()
        {
            var model =new ManagersViewModel(){
                ManagerTasks = new List<ManagerTasksViewModel>
                            {
                                new ManagerTasksViewModel()
                                    {
                                        Manager = new Employee() {Id = 100, Name = "Vasya"},
                                        Tasks =
                                            new List<HumanTask>
                                                {
                                                    new HumanTask {Id = 1, Name = "Do Something task 1",Description = "bla bla bla"},
                                                    new HumanTask {Id = 2, Name = "Do Something task 2",Description = "bla bla bla"},
                                                    new HumanTask {Id = 3, Name = "Do Something task 3",Description = "bla bla bla"},
                                                    new HumanTask {Id = 4, Name = "Do Something task 4",Description = "bla bla bla"},
                                                    new HumanTask {Id = 5, Name = "Do Something task 5",Description = "bla bla bla"},
                                                }
                                    },
                                new ManagerTasksViewModel()
                                    {
                                        Manager = new Employee() {Id = 101, Name = "Petya"},
                                        Tasks =
                                            new List<HumanTask>
                                                {
                                                    new HumanTask {Id = 6, Name = "Do Something task 6",Description = "bla bla bla"},
                                                    new HumanTask {Id = 7, Name = "Do Something task 7",Description = "bla bla bla"},
                                                    new HumanTask {Id = 8, Name = "Do Something task 8",Description = "bla bla bla"},
                                                    new HumanTask {Id = 9, Name = "Do Something task 9",Description = "bla bla bla"},
                                                    new HumanTask {Id = 10, Name = "Do Something task 10",Description = "bla bla bla"},
                                                }
                                    },
                                new ManagerTasksViewModel()
                                    {
                                        Manager = new Employee() {Id = 102, Name = "Vanya"},
                                        Tasks =
                                            new List<HumanTask>
                                                {
                                                    new HumanTask {Id = 11, Name = "Do Something task 11",Description = "bla bla bla"},
                                                    new HumanTask {Id = 12, Name = "Do Something task 12",Description = "bla bla bla"},
                                                    new HumanTask {Id = 13, Name = "Do Something task 13",Description = "bla bla bla"},
                                                    new HumanTask {Id = 14, Name = "Do Something task 14",Description = "bla bla bla"},
                                                    new HumanTask {Id = 15, Name = "Do Something task 15",Description = "bla bla bla"},
                                                }
                                    },
                            },
                            UnAssignedTasks = new List<HumanTask>()
                            { 
                                new HumanTask() {Name = "Pick Some One task 16",Description = "do do do do"},
                                new HumanTask() {Name = "Pick Some One task 17",Description = "do do do do"},
                                new HumanTask() {Name = "Pick Some One task 18",Description = "do do do do"},
                            }
                        }; 
            
            return model;
        }
    }
}