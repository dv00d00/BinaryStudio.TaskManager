using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BinaryStudio.TaskManager.Logic.Core;
using BinaryStudio.TaskManager.Logic.Domain;
using BinaryStudio.TaskManager.Web.Models;

namespace BinaryStudio.TaskManager.Web.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ITaskProcessor taskProcessor;

        private readonly IUserProcessor userProcessor;

        public readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="HumanTasksController"/> class.
        /// </summary>
        /// <param name="taskProcessor">The task processor.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="userProcessor">The user processor. </param>
        public ProjectController(ITaskProcessor taskProcessor, IUserProcessor userProcessor, IUserRepository userRepository)
        {
            this.taskProcessor = taskProcessor;
            this.userProcessor = userProcessor;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Creates the specified user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public ActionResult CreateTask(int userId)
        {
            var humanTask = new HumanTask();
            humanTask.AssigneeId = (userId != -1) ? userId : (int?)null;
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
        public ActionResult CreateTask(HumanTask humanTask)
        {
            humanTask.Assigned = humanTask.AssigneeId == (int?)null ? humanTask.Created : (DateTime?)null;
            if (this.ModelState.IsValid)
            {
                this.taskProcessor.CreateTask(humanTask);
                return this.RedirectToAction("PersonalProject");
            }
            return this.View(humanTask);
        }

        [Authorize]
        public ActionResult PersonalProject()
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