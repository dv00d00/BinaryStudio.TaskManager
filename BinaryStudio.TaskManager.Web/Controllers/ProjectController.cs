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
        /// <param name="projectRepository">
        /// The project repository.
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
            var humanTask = new HumanTask();
            humanTask.AssigneeId = (userId != -1) ? userId : (int?)null;
            humanTask.CreatorId = this.userProcessor.GetCurrentLoginedUser(User.Identity.Name).Id;
            humanTask.Created = DateTime.Now;
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
            int projectId = 1;
            if (this.ModelState.IsValid)
            {
                humanTask.Assigned = humanTask.AssigneeId == (int?)null ? humanTask.Created : (DateTime?)null;
                humanTask.Project = this.projectRepository.GetById(projectId);
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
                    UsersTasks = new List<ManagerTasksViewModel>(),
                    UnAssignedTasks = this.taskProcessor.GetUnassignedTasks().ToList()
                };
            var users = this.projectRepository.GetAllUsersInProject(projectId);
            foreach (var user in users)
            {
                var managerModel = new ManagerTasksViewModel
                    {
                        User = user,
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

        /// <summary>
        /// The invite or delete user.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult InviteOrDeleteUser()
        {
            return this.View(this.userRepository.GetAll());
        }

        /// <summary>
        /// The add user.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult AddUser(int userId, int projectId)
        {
            var user = this.userRepository.GetById(userId);
            user.UserProjects.Add(this.projectRepository.GetById(projectId));
            this.userRepository.UpdateUser(user);

            var project = this.projectRepository.GetById(projectId);
            project.ProjectUsers.Add(this.userRepository.GetById(userId));
            this.projectRepository.Update(project);
            
            return this.RedirectToAction("PersonalProject");
        }

        /// <summary>
        /// The delete user.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult DeleteUser(int userId, int projectId)
        {
            var user = this.userRepository.GetById(userId);
            user.UserProjects.Remove(this.projectRepository.GetById(projectId));
            this.userRepository.UpdateUser(user);

            var project = this.projectRepository.GetById(projectId);
            project.ProjectUsers.Remove(this.userRepository.GetById(userId));
            this.projectRepository.Update(project);

            return this.RedirectToAction("PersonalProject");
        }

        /// <summary>
        /// The get image.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult GetImage(int userId)
        {
            User user = this.userRepository.GetById(userId);
            if (user != null)
            {
                return this.File(user.ImageData, user.ImageMimeType);
            }

            return null;
        }

        /// <summary>
        /// The all tasks.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ViewResult.
        /// </returns>
        [Authorize]
        public ViewResult AllTasks()
        {
            var model = new List<SingleTaskViewModel>();
            string creatorName, assigneeName;
            int projectId = 1;
            var tasks = this.taskProcessor.GetAllTasksInProject(projectId).ToList();
            foreach (var task in tasks)
            {
                // TODO: fix usrRepo for userProcessor
                creatorName = task.CreatorId.HasValue ? this.userRepository.GetById((int)task.CreatorId).UserName : "none";
                assigneeName = task.AssigneeId.HasValue ? this.userRepository.GetById((int)task.AssigneeId).UserName : "none";
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
        /// The edit.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        public ActionResult Edit(int id)
        {
            var humantask = this.taskProcessor.GetTaskById(id);
            this.ViewBag.PossibleCreators = new List<User>();
            this.ViewBag.PossibleAssignees = new List<User>();
            return this.View(humantask);
        }

        /// <summary>
        /// The details.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        public ActionResult Details(int id)
        {
            var model = this.CreateSingleTaskViewModelById(id);
            return this.View(model);
        }

        /// <summary>
        /// The create single task view model by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The BinaryStudio.TaskManager.Web.Models.SingleTaskViewModel.
        /// </returns>
        private SingleTaskViewModel CreateSingleTaskViewModelById(int id)
        {
            var model = new SingleTaskViewModel();
            var task = this.taskProcessor.GetTaskById(id);
            var creatorName = task.CreatorId.HasValue
                                  ? this.userRepository.GetById((int)task.CreatorId).UserName
                                  : "none";
            var assigneeName = task.AssigneeId.HasValue
                                   ? this.userRepository.GetById((int)task.AssigneeId).UserName
                                   : "none";
            model.HumanTask = task;
            model.CreatorName = creatorName;
            model.AssigneeName = assigneeName;
            model.TaskHistories = this.taskProcessor.GetAllHistoryForTask(id).OrderByDescending(x => x.ChangeDateTime).ToList();
            return model;
        }
    }
}