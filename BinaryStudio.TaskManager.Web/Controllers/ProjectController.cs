<<<<<<< HEAD
﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectController.cs" company="">
//   
// </copyright>
// <summary>
//   The project controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BinaryStudio.TaskManager.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;
    using BinaryStudio.TaskManager.Web.Models;

    /// <summary>
    /// The project controller.
    /// </summary>
    public class ProjectController : Controller
    {
        /// <summary>
        /// The task processor.
        /// </summary>
        private readonly ITaskProcessor taskProcessor;

        /// <summary>
        /// The user processor.
        /// </summary>
        private readonly IUserProcessor userProcessor;

        /// <summary>
        /// The user repository.
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// The project repository.
        /// </summary>
        private readonly IProjectRepository projectRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectController"/> class.         
        /// </summary>
        /// <param name="taskProcessor">
        /// The task processor.
        /// </param>
        /// <param name="userProcessor">
        /// The user processor. 
        /// </param>
        /// <param name="userRepository">
        /// The user repository.
        /// </param>
        public ProjectController(ITaskProcessor taskProcessor, IUserProcessor userProcessor, IUserRepository userRepository, IProjectRepository projectRepository)
        {
            this.taskProcessor = taskProcessor;
            this.userProcessor = userProcessor;
            this.userRepository = userRepository;
            this.projectRepository = projectRepository;
        }

        /// <summary>
        /// The create task.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult CreateTask(int userId)
        {
            var humanTask = new HumanTask
                {
                    AssigneeId = (userId != -1) ? userId : (int?)null,
                    CreatorId = this.userProcessor.GetCurrentLoginedUser(this.User.Identity.Name).Id,
                    Created = DateTime.Now
                };
            return this.View(humanTask);
        }

        /// <summary>
        /// The create task.
        /// </summary>
        /// <param name="humanTask">
        /// The human task.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult CreateTask(HumanTask humanTask)
        {
            if (this.ModelState.IsValid)
            {
                humanTask.Assigned = humanTask.AssigneeId == (int?)null ? humanTask.Created : (DateTime?)null;
                this.taskProcessor.CreateTask(humanTask);
                return this.RedirectToAction("PersonalProject");
            }

            return this.View(humanTask);
        }

        /// <summary>
        /// The personal project.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        public ActionResult PersonalProject()
        {
            int projectId = 1;
            var model = new ProjectViewModel
                {
                    UsersTasks= new List<ManagerTasksViewModel>(),
                    UnAssignedTasks = this.taskProcessor.GetUnassignedTasks().ToList()
                };
            var users = this.projectRepository.GetAllUsersInProject(projectId);
            foreach (var user in users)
            {
                var managerModel = new ManagerTasksViewModel
                    {
                        Manager = user.User,
                        Tasks = this.taskProcessor.GetTasksList(user.Id).ToList()
                    };
                model.UsersTasks.Add(managerModel);
            }

            return this.View(model);
        }

        /// <summary>
        /// The move task.
        /// </summary>
        /// <param name="taskId">
        /// The task id.
        /// </param>
        /// <param name="senderId">
        /// The sender id.
        /// </param>
        /// <param name="receiverId">
        /// The receiver id.
        /// </param>
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
            this.taskProcessor.MoveTaskToUnassigned(taskId);
        }

        public ActionResult InviteUser()
        {
            return this.View(this.userRepository.GetAll());
        }

        public ActionResult AddUser(int UserId, int ProjectId)
        {
            ProjectsAndUsers projectsAndUsers = new ProjectsAndUsers();
            projectsAndUsers.User = this.userRepository.GetById(UserId);
            projectsAndUsers.Project = this.projectRepository.GetById(ProjectId);
            return this.RedirectToAction("PersonalProject");
        }

        [HttpPost]
        public ActionResult AddUser(ProjectsAndUsers projectsAndUsers)
        {
            int projectId = 1;
            this.projectRepository.GetAllUsersInProject(projectId);
            return this.RedirectToAction("PersonalProject");
        }
    }
=======
﻿namespace BinaryStudio.TaskManager.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;
    using BinaryStudio.TaskManager.Web.Models;

    /// <summary>
    /// The project controller.
    /// </summary>
    public class ProjectController : Controller
    {
        /// <summary>
        /// The task processor.
        /// </summary>
        private readonly ITaskProcessor taskProcessor;

        /// <summary>
        /// The user processor.
        /// </summary>
        private readonly IUserProcessor userProcessor;

        /// <summary>
        /// The user repository.
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectController"/> class.         
        /// </summary>
        /// <param name="taskProcessor">
        /// The task processor.
        /// </param>
        /// <param name="userProcessor">
        /// The user processor. 
        /// </param>
        /// <param name="userRepository">
        /// The user repository.
        /// </param>
        public ProjectController(ITaskProcessor taskProcessor, IUserProcessor userProcessor, IUserRepository userRepository)
        {
            this.taskProcessor = taskProcessor;
            this.userProcessor = userProcessor;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// The create task.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult CreateTask(int userId)
        {
            var humanTask = new HumanTask
                {
                    AssigneeId = (userId != -1) ? userId : (int?)null,
                    CreatorId = this.userProcessor.GetCurrentLoginedUser(this.User.Identity.Name).Id,
                    Created = DateTime.Now
                };
            return this.View(humanTask);
        }

        /// <summary>
        /// The create task.
        /// </summary>
        /// <param name="humanTask">
        /// The human task.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult CreateTask(HumanTask humanTask)
        {
            if (this.ModelState.IsValid)
            {
                humanTask.Assigned = humanTask.AssigneeId == (int?)null ? humanTask.Created : (DateTime?)null;
                this.taskProcessor.CreateTask(humanTask);
                return this.RedirectToAction("PersonalProject");
            }

            return this.View(humanTask);
        }

        /// <summary>
        /// The personal project.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        public ActionResult PersonalProject()
        {
            var model = new ProjectViewModel
                {
                    UsersTasks = new List<ManagerTasksViewModel>(),
                    UnAssignedTasks = this.taskProcessor.GetUnassignedTasks().ToList()
                };
            var users = this.userRepository.GetAll();
            foreach (var user in users)
            {
                var managerModel = new ManagerTasksViewModel
                    {
                        Manager = user,
                        Tasks = this.taskProcessor.GetTasksList(user.Id).ToList()
                    };
                model.UsersTasks.Add(managerModel);
            }

            return this.View(model);
        }

        /// <summary>
        /// The move task.
        /// </summary>
        /// <param name="taskId">
        /// The task id.
        /// </param>
        /// <param name="senderId">
        /// The sender id.
        /// </param>
        /// <param name="receiverId">
        /// The receiver id.
        /// </param>
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
            this.taskProcessor.MoveTaskToUnassigned(taskId);
        }
    }
>>>>>>> 8f790d08cc776362d57289bb64ea5d782069e6a0
}