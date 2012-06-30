using System;
using System.Collections;
using System.Collections.ObjectModel;
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
        private readonly ITaskProcessor taskProcessor;

        private readonly IEmployeeRepository employeeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="HumanTasksController"/> class.
        /// </summary>
        /// <param name="taskProcessor">The task processor.</param>
        public HumanTasksController(ITaskProcessor taskProcessor, IEmployeeRepository employeeRepository)
        {
            this.taskProcessor = taskProcessor;
            this.employeeRepository = employeeRepository;
        }

        //
        // GET: /HumanTasks/

        public ViewResult Index()
        {
            var humanTasks = this.taskProcessor.GetAllTasks();

            return View(humanTasks);
        }

        //
        // GET: /HumanTasks/Details/5

        public ViewResult Details(int id)
        {
            HumanTask humantask = this.taskProcessor.GetTaskById(id);
            return View(humantask);
        }

        //
        // GET: /HumanTasks/Create
       
        public ActionResult Create(int managerId)
        {
            HumanTask humanTask = new HumanTask();
            humanTask.AssigneeId = (managerId != -1) ? managerId : (int?) null;
            humanTask.CreatorId = humanTask.AssigneeId;
            //TODO: creator pull from logon screen                

            humanTask.Created = DateTime.Now;

            return View(humanTask);
        }

        //
        // POST: /HumanTasks/Create

        [HttpPost]
        public ActionResult Create(HumanTask humanTask)
        {

            humanTask.Assigned = humanTask.AssigneeId == (int?) null ? humanTask.Created : (DateTime?) null;
            if (ModelState.IsValid)
            {
                this.taskProcessor.CreateTask(humanTask);
                return RedirectToAction("AllManagersWithTasks");
            }

            //TODO: refactor this "PossibleCreators" and "PossibleAssignees"
            ViewBag.PossibleCreators = new List<Employee>();
            ViewBag.PossibleAssignees = new List<Employee>();
            
            return View(humanTask);
        }

        //
        // GET: /HumanTasks/Edit/5

        public ActionResult Edit(int id)
        {
            HumanTask humantask = this.taskProcessor.GetTaskById(id);
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
                this.taskProcessor.UpdateTask(humanTask);
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
            HumanTask humantask = this.taskProcessor.GetTaskById(id);
            return View(humantask);
        }

        //
        // POST: /HumanTasks/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            this.taskProcessor.DeleteTask(id);
            return RedirectToAction("Index");
        }

        public ActionResult ManagerDetails(int managerId)
        {
            ViewBag.ManagerName = employeeRepository.GetById(managerId).Name;
            ViewBag.ManagerId = managerId;
            TaskViewModel taskViewModel = new TaskViewModel();
            IList<TaskViewModel> model = new List<TaskViewModel>();
            IList<HumanTask> humanTasks = new List<HumanTask>();
            humanTasks = taskProcessor.GetTasksList(managerId).ToList();
            foreach (var task in humanTasks)
            {
                model.Add(
                            new TaskViewModel()
                                {
                                    Task = task,
                                    CreatorName = task.CreatorId.HasValue?employeeRepository.GetById(task.CreatorId.Value).Name:""
                                }
                    );
            }
            return View(model);
        }

        public ActionResult AllManagersWithTasks()
        {
            ManagersViewModel model = new ManagersViewModel();
            model.ManagerTasks = new List<ManagerTasksViewModel>();
            model.UnAssignedTasks = taskProcessor.GetUnassignedTasks().ToList();
            IList<Employee> employees = employeeRepository.GetAll();
            foreach (Employee employee in employees)
            {
                ManagerTasksViewModel manager = new ManagerTasksViewModel();
                manager.Manager = employee;
                manager.Tasks = taskProcessor.GetTasksList(employee.Id).ToList();
                model.ManagerTasks.Add(manager);
            }
            return View(model);
        }

        //value 0 - tasks's
        //value 1 - current task owner
        //value 2 - move to
        public void MoveTask(List<int> values)
        {
        
            // move to real manager
            if(values[2] != -1)
            {
                taskProcessor.MoveTask(values[0], values[2]);
                return;
            }
            //make task unassigned
            if(values[2]==-1)
            {
                taskProcessor.MoveTaskToUnassigned(values[0]);
            }

            
        }
    }
}