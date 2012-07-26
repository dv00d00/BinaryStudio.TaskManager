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
        /// The project processor.
        /// </summary>
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
        /// <param name="projectProcessor">
        /// The project processor.
        /// </param>
        public ProjectController(ITaskProcessor taskProcessor, IUserProcessor userProcessor, IProjectProcessor projectProcessor)
        {
            this.projectProcessor = projectProcessor;
            this.taskProcessor = taskProcessor;
            this.userProcessor = userProcessor;
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
                    CreatorId = this.userProcessor.GetUserByName(this.User.Identity.Name).Id,
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
            const int ProjectId = 1;
            var model = new ProjectViewModel
                {
                    UsersTasks = new List<ManagerTasksViewModel>(),
                    UnAssignedTasks = this.taskProcessor.GetUnassignedTasks().ToList()
                };
            var users = this.projectProcessor.GetAllUsersInProject(ProjectId);
            foreach (var user in users)
            {
                var viewModel = new ManagerTasksViewModel
                    {
                        User = user,
                        Tasks = this.taskProcessor.GetTasksList(user.Id).ToList()
                    };
                model.UsersTasks.Add(viewModel);
            }

            return this.View(model);
        }

        /// <summary>
        /// The move task from one user to another.
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
            var model = new ProjectCollaboratorsViewModel();
            var currentUser = this.userProcessor.GetUserByName(User.Identity.Name);
            var users = this.userProcessor.GetAllUsers();
            var list = new List<User>();
            list.Add(currentUser);
            model.Collaborators = users.Except(list);
            return this.View(model);
        }

        /// <summary>
        /// The invite user in project.
        /// </summary>
        /// <param name="senderId">
        /// The sender Id.
        /// </param>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <param name="receiverId">
        /// The receiver Id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult InviteUserInProject(int receiverId, int projectId)
        {
            var senderId = this.userProcessor.GetUserByName(User.Identity.Name).Id;
            this.projectProcessor.InviteUserInProject(senderId, projectId, receiverId);
            return this.RedirectToAction("PersonalProject");
        }

        /// <summary>
        /// The remove user from project.
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
        public ActionResult RemoveUserFromProject(int userId, int projectId)
        {
            this.projectProcessor.RemoveUserFromProject(userId, projectId);
            return this.RedirectToAction("PersonalProject");
        }

        public ActionResult Invitations()
        {
            var user = this.userProcessor.GetUserByName(User.Identity.Name);
            var invitationsToUser = this.projectProcessor.GetAllInvitationsToUser(user.Id);
            var model = invitationsToUser.Select(invitation => new InvitationsViewModel { Invitation = invitation, Sender = invitation.Sender, Project = invitation.Project }).ToList();
            return this.View(model);
        }
    }
}