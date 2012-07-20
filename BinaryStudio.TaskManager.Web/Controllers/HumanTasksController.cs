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
        
        private readonly IUserProcessor userProcessor;

        public readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="HumanTasksController"/> class.
        /// </summary>
        /// <param name="taskProcessor">The task processor.</param>
        /// <param name="employeeRepository">The employee repository.</param>
        /// <param name="userProcessor">The user processor. </param>
        public HumanTasksController(ITaskProcessor taskProcessor, IUserProcessor userProcessor, IUserRepository userRepository)
        {
            this.taskProcessor = taskProcessor;            
            this.userProcessor = userProcessor;
            this.userRepository = userRepository;
        }
        
        /// <summary>
        /// All the tasks.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ViewResult AllTasks()
        {
            var model = new List<SingleTaskViewModel>();
            string creatorName, assigneeName;
            var tasks = this.taskProcessor.GetAllTasks().ToList();
            foreach (var task in tasks)
            {                
                creatorName = task.CreatorId.HasValue ? this.userRepository.GetById((int)task.CreatorId).UserName : "none";
                assigneeName = task.AssigneeId.HasValue ? this.userRepository.GetById((int) task.AssigneeId).UserName : "none";
                model.Add(new SingleTaskViewModel
                              {
                                  HumanTask = task,
                                  AssigneeName = assigneeName,
                                  CreatorName = creatorName
                              });
            }

            return View(model);
        }

        /// <summary>
        /// Detailses the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [Authorize]
        public ViewResult Details(int id)
        {
            var model = CreateSingleTaskViewModelById(id);
            return View(model);
        }

        /// <summary>
        /// Creates the specified manager id.
        /// </summary>
        /// <param name="managerId">The manager id.</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Create(int managerId)
        {
            HumanTask humanTask = new HumanTask();
            humanTask.AssigneeId = (managerId != -1) ? managerId : (int?)null;
            humanTask.CreatorId = userProcessor.GetCurrentLoginedUser(User.Identity.Name).Id;         
            humanTask.Created = DateTime.Now;
            return this.View(humanTask);
        }

        /// <summary>
        /// Creates the specified human task.
        /// </summary>
        /// <param name="humanTask">The human task.</param>
        /// <returns></returns>
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
            this.ViewBag.PossibleCreators = new List<User>();
            this.ViewBag.PossibleAssignees = new List<User>();

            return this.View(humanTask);
        }

        /// <summary>
        /// Edits the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit(int id)
        {
            HumanTask humantask = this.taskProcessor.GetTaskById(id);
            this.ViewBag.PossibleCreators = new List<User>();
            this.ViewBag.PossibleAssignees = new List<User>();
            return this.View(humantask);
        }

        /// <summary>
        /// Edits the specified human task.
        /// </summary>
        /// <param name="humanTask">The human task.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult Edit(HumanTask humanTask)
        {
            if (this.ModelState.IsValid)
            {
                this.taskProcessor.UpdateTask(humanTask);
                return this.RedirectToAction("AllManagersWithTasks");
            }

            this.ViewBag.PossibleCreators = new List<User>();
            this.ViewBag.PossibleAssignees = new List<User>();
            return this.View(humanTask);
        }

        /// <summary>
        /// Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Delete(int id)
        {
            var model = CreateSingleTaskViewModelById(id);
            return this.View(model);
        }

        private SingleTaskViewModel CreateSingleTaskViewModelById(int id)
        {
            var model = new SingleTaskViewModel();
            var task = this.taskProcessor.GetTaskById(id);
            var creatorName = task.CreatorId.HasValue ? this.userRepository.GetById((int) task.CreatorId).UserName : "none";
            var assigneeName = task.AssigneeId.HasValue ? this.userRepository.GetById((int) task.AssigneeId).UserName : "none";
            model.HumanTask = task;
            model.CreatorName = creatorName;
            model.AssigneeName = assigneeName;
            return model;
        }

        /// <summary>
        /// Deletes the confirmed.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Delete")]        
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            this.taskProcessor.DeleteTask(id);
            return RedirectToAction("AllTasks");
        }

        [Authorize]
        public ActionResult ManagerDetails(int managerId)
        {
            this.ViewBag.ManagerName = this.userRepository.GetById(managerId).UserName;
            this.ViewBag.ManagerId = managerId;
            IList<TaskViewModel> model = new List<TaskViewModel>();
            IList<HumanTask> humanTasks = new List<HumanTask>();
            humanTasks = this.taskProcessor.GetTasksList(managerId).ToList();
            foreach (var task in humanTasks)
            {
                model.Add(
                            new TaskViewModel()
                                {
                                    Task = task,
                                    CreatorName =
                                        task.CreatorId.HasValue
                                            ? this.userRepository.GetById(task.CreatorId.Value).UserName
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
            var users = this.userRepository.GetAll();
            foreach (var user in users)
            {
                var managerModel = new ManagerTasksViewModel();
                managerModel.Manager = user;
                managerModel.Tasks = this.taskProcessor.GetTasksList(user.Id).ToList();
                model.ManagerTasks.Add(managerModel);
            }
            return this.View(model);
        }

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

    }
}