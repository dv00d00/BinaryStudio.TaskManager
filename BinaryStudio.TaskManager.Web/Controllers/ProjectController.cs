// --------------------------------------------------------------------------------------------------------------------
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

        private readonly IProjectProcessor projectProcessor;

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
        /// <param name="projectProcessor"> </param>
        public ProjectController(ITaskProcessor taskProcessor, IUserProcessor userProcessor, IUserRepository userRepository, IProjectRepository projectRepository, IProjectProcessor projectProcessor)
        {
            this.projectProcessor = projectProcessor;
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
                    UsersTasks = new List<ManagerTasksViewModel>(),
                    UnAssignedTasks = this.taskProcessor.GetUnassignedTasks().ToList()
                };
            var users = this.projectRepository.GetAllUsersInProject(projectId);//this.userRepository.GetAll();
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

        public ActionResult InviteOrDeleteUser()
        {
            return this.View(this.userProcessor.GetAllUsers());
        }

        public ActionResult AddUser(int UserId, int ProjectId)
        {
            this.projectProcessor
            var user = this.userRepository.GetById(UserId);
            user.UserProjects.Add(this.projectRepository.GetById(ProjectId));
            this.userRepository.UpdateUser(user);

            var project = this.projectRepository.GetById(ProjectId);
            project.ProjectUsers.Add(this.userRepository.GetById(UserId));
            this.projectRepository.Update(project);
            
            return this.RedirectToAction("PersonalProject");
        }

        public ActionResult DeleteUser(int UserId, int ProjectId)
        {
            var user = this.userRepository.GetById(UserId);
            user.UserProjects.Remove(this.projectRepository.GetById(ProjectId));
            this.userRepository.UpdateUser(user);

            var project = this.projectRepository.GetById(ProjectId);
            project.ProjectUsers.Remove(this.userRepository.GetById(UserId));
            this.projectRepository.Update(project);

            return this.RedirectToAction("PersonalProject");
        }
    }
}