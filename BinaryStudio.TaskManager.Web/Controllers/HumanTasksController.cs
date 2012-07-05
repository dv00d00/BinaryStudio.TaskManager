namespace BinaryStudio.TaskManager.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;
    using BinaryStudio.TaskManager.Web.Models;

    using NLog;

    /// <summary>
    /// Provides access to human task entities
    /// </summary>
    public class HumanTasksController : Controller
    {
        private readonly ITaskProcessor taskProcessor;

        private readonly IEmployeeRepository employeeRepository;

        private readonly Logger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref="HumanTasksController"/> class.
        /// </summary>
        /// <param name="taskProcessor">The task processor.</param>
        /// <param name="employeeRepository">The employee repository.</param>
        public HumanTasksController(ITaskProcessor taskProcessor, IEmployeeRepository employeeRepository)
        {
            this.taskProcessor = taskProcessor;
            this.employeeRepository = employeeRepository;
        }

        // GET: /HumanTasks/
        [Authorize]
        public ViewResult AllTasks()
        {
            var model = new List<AllTasksViewModel>();
            string creatorName, assigneeName;
            foreach (var task in this.taskProcessor.GetAllTasks().ToList())
            {                
                creatorName = task.CreatorId.HasValue ? this.employeeRepository.GetById((int)task.CreatorId).Name : "none";
                assigneeName = task.AssigneeId.HasValue ? this.employeeRepository.GetById((int) task.AssigneeId).Name : "none";
                model.Add(new AllTasksViewModel
                              {
                                  HumanTask = task,
                                  AssigneeName = assigneeName,
                                  CreatorName = creatorName
                              });
            }

            return this.View(model);
        }

        // GET: /HumanTasks/DetailsEmployee/5
        [Authorize]
        public ViewResult Details(int id)
        {
            var humantask = this.taskProcessor.GetTaskById(id);
            return this.View(humantask);
        }

        // GET: /HumanTasks/Create
        [Authorize]
        public ActionResult Create(int managerId)
        {
            var humanTask = new HumanTask();
            humanTask.AssigneeId = (managerId != -1) ? managerId : (int?)null;
            humanTask.CreatorId = humanTask.AssigneeId;
            //TODO: creator pull from logon screen                

            humanTask.Created = DateTime.Now;
            return this.View(humanTask);
        }


        // POST: /HumanTasks/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(HumanTask humanTask)
        {

            humanTask.Assigned = humanTask.AssigneeId == (int?)null ? humanTask.Created : (DateTime?)null;
            if (this.ModelState.IsValid)
            {
                this.taskProcessor.CreateTask(humanTask);
                return this.RedirectToAction("AllManagersWithTasks");
            }

            //TODO: refactor this "PossibleCreators" and "PossibleAssignees"
            this.ViewBag.PossibleCreators = new List<Employee>();
            this.ViewBag.PossibleAssignees = new List<Employee>();

            return this.View(humanTask);
        }

        // GET: /HumanTasks/EditEmployee/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var humantask = this.taskProcessor.GetTaskById(id);
            this.ViewBag.PossibleCreators = new List<Employee>();
            this.ViewBag.PossibleAssignees = new List<Employee>();
            return this.View(humantask);
        }

        // POST: /HumanTasks/EditEmployee/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(HumanTask humanTask)
        {
            if (this.ModelState.IsValid)
            {
                this.taskProcessor.UpdateTask(humanTask);
                return this.RedirectToAction("Index");
            }

            this.ViewBag.PossibleCreators = new List<Employee>();
            this.ViewBag.PossibleAssignees = new List<Employee>();
            return this.View(humanTask);
        }

        // GET: /HumanTasks/DeleteEmployee/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            var humantask = this.taskProcessor.GetTaskById(id);
            return this.View(humantask);
        }

        // POST: /HumanTasks/DeleteEmployee/5
        [HttpPost, ActionName("DeleteEmployee")]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            this.taskProcessor.DeleteTask(id);
            return this.RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult ManagerDetails(int managerId)
        {
            this.ViewBag.ManagerName = this.employeeRepository.GetById(managerId).Name;
            this.ViewBag.ManagerId = managerId;
            var model = new List<TaskViewModel>();
            var humanTasks = new List<HumanTask>();
            humanTasks = this.taskProcessor.GetTasksList(managerId).ToList();
            foreach (var task in humanTasks)
            {
                model.Add(
                            new TaskViewModel()
                                {
                                    Task = task,
                                    CreatorName =
                                        task.CreatorId.HasValue
                                            ? this.employeeRepository.GetById(task.CreatorId.Value).Name
                                            : ""
                                });
            }
            return this.View(model);
        }

        [Authorize]
        public ActionResult AllManagersWithTasks()
        {
            var model = new ManagersViewModel();
            model.ManagerTasks = new List<ManagerTasksViewModel>();
            model.UnAssignedTasks = this.taskProcessor.GetUnassignedTasks().ToList();
            var employees = this.employeeRepository.GetAll();
            foreach (var employee in employees)
            {
                var managerModel = new ManagerTasksViewModel();
                managerModel.Manager = employee;
                managerModel.Tasks = this.taskProcessor.GetTasksList(employee.Id).ToList();
                model.ManagerTasks.Add(managerModel);
            }
            return this.View(model);
        }

        //value 0 - tasks's
        //value 1 - current task owner
        //value 2 - move to
        [Authorize]
        public void MoveTask(int taskId, int senderId, int receiverId)
        {
            // move to real manager
            if (receiverId != -1)
            {
                this.taskProcessor.MoveTask(taskId, receiverId);
                return;
            }

            // make task unassigned
            if (receiverId == -1)
            {
                this.taskProcessor.MoveTaskToUnassigned(taskId);
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null) return;

            var exception = filterContext.Exception ?? new Exception("No further information");
            this.log.DebugException("EXCEPTION", exception);

            filterContext.ExceptionHandled = true;
            filterContext.Result = this.View("Error");
        }
    }
}