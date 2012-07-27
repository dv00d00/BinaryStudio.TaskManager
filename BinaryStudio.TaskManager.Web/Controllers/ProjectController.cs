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
    [Authorize]
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

        [Authorize]
        public ActionResult Project(int id)
        {
            var model = new ProjectViewModel
            {
                UsersTasks = new List<ManagerTasksViewModel>(),
                UnAssignedTasks = this.taskProcessor.GetUnAssignedTasksForProject(id).ToList(),
                ProjectId = id
            };
            var users = this.projectProcessor.GetAllUsersInProject(id).Reverse();
            foreach (var user in users)
            {
                var managerModel = new ManagerTasksViewModel();
                managerModel.User = user;
                managerModel.Tasks = this.taskProcessor.GetAllTasksForUserInProject(id, user.Id).ToList();
                model.UsersTasks.Add(managerModel);
            }
            return this.View(model);
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
        public ActionResult CreateTask(int userId, int projectId)
        {
            var createModel = new CreateTaskViewModel
            {
                Priorities = this.taskProcessor.GetPrioritiesList().OrderBy(x => x.Value),
                AssigneeId = (userId != -1) ? userId : (int?)null,
                CreatorId = this.userProcessor.GetUserByName(User.Identity.Name).Id,
                Created = DateTime.Now,
                ProjectId = projectId
            };
            return this.View(createModel);
        }

        /// <summary>
        /// The create task.
        /// </summary>
        /// <param name="createModel">
        /// The human task.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        public ActionResult CreateTask(CreateTaskViewModel createModel)
        {
            createModel.Assigned = createModel.AssigneeId == (int?)null ? createModel.Created : (DateTime?)null;
            if (this.ModelState.IsValid)
            {
                var task = new HumanTask
                {
                    Assigned = createModel.Assigned,
                    AssigneeId = createModel.AssigneeId,
                    Closed = createModel.Closed,
                    Finished = createModel.Finished,
                    Created = createModel.Created,
                    CreatorId = createModel.CreatorId,
                    Description = createModel.Description,
                    Id = createModel.Id,
                    Name = createModel.Name,
                    Priority = createModel.Priority,
                    ProjectId = createModel.ProjectId,
                };
                this.taskProcessor.CreateTask(task);
                this.taskProcessor.AddHistory(new HumanTaskHistory
                {
                    NewDescription = task.Description,
                    ChangeDateTime = DateTime.Now,
                    NewAssigneeId = task.AssigneeId,
                    NewName = task.Name,
                    Task = task,
                    NewPriority = task.Priority,
                });
                return this.RedirectToAction("Project",new {id = createModel.ProjectId});
            }
            createModel.Priorities = taskProcessor.GetPrioritiesList();
            // TODO: refactor this "PossibleCreators" and "PossibleAssignees"
            this.ViewBag.PossibleCreators = new List<User>();
            this.ViewBag.PossibleAssignees = new List<User>();

            return this.View(createModel);
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
            foreach (var viewModel in users.Select(user => new ManagerTasksViewModel
                {
                    User = user,
                    Tasks = this.taskProcessor.GetTasksList(user.Id).ToList()
                }))
            {
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
            // move to real user
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
            var currentUser = this.userProcessor.GetUserByName(User.Identity.Name);
            var users = this.userProcessor.GetAllUsers();
            var listWithCurrentUser = new List<User> { currentUser };
            var model = new ProjectCollaboratorsViewModel { Collaborators = users.Except(listWithCurrentUser) };
            return this.View(model);
        }

        /// <summary>
        /// The invite user in project.
        /// </summary>
        /// <param name="receiverId">
        /// The receiver id.
        /// </param>
        /// <param name="projectId">
        /// The project id.
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

        /// <summary>
        /// The invitations.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult Invitations()
        {
            var user = this.userProcessor.GetUserByName(User.Identity.Name);
            var invitationsToUser = this.projectProcessor.GetAllInvitationsToUser(user.Id);

            var model = invitationsToUser.Select(invitation => new InvitationsViewModel { Invitation = invitation, Sender = invitation.Sender, Project = invitation.Project }).ToList();
            return this.View(model);
        }

        /// <summary>
        /// The submit invitation in project.
        /// </summary>
        /// <param name="invitationId">
        /// The invitation id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult SubmitInvitationInProject(int invitationId)
        {            
            this.projectProcessor.ConfirmInvitationInProject(invitationId);
            return this.RedirectToAction("Invitations");
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
            User user = this.userProcessor.GetUser(userId);
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
                creatorName = task.CreatorId.HasValue ? this.userProcessor.GetUser((int)task.CreatorId).UserName : "none";
                assigneeName = task.AssigneeId.HasValue ? this.userProcessor.GetUser((int)task.AssigneeId).UserName : "none";
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
        /// The edit.
        /// </summary>
        /// <param name="humanTask">
        /// The human task.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]        
        public ActionResult Edit(HumanTask humanTask)
        {
            if (this.ModelState.IsValid)
            {

                this.taskProcessor.UpdateTask(humanTask);
                this.taskProcessor.AddHistory(new HumanTaskHistory
                {
                    NewDescription = humanTask.Description,
                    ChangeDateTime = DateTime.Now,
                    NewAssigneeId = humanTask.AssigneeId,
                    NewName = humanTask.Name,
                    Task = humanTask,
                    NewPriority = humanTask.Priority,
                });

                return this.RedirectToAction("PersonalProject");
            }

            this.ViewBag.PossibleCreators = new List<User>();
            this.ViewBag.PossibleAssignees = new List<User>();
            return this.View(humanTask);
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
        public ActionResult Details(int id)
        {
            var model = this.CreateSingleTaskViewModelById(id);
            return this.View(model);
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult Delete(int id)
        {
            var model = this.CreateSingleTaskViewModelById(id);
            return this.View(model);
        }

        /// <summary>
        /// The delete confirmed.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        [ActionName("Delete")]        
        public ActionResult DeleteConfirmed(int id)
        {
            this.taskProcessor.DeleteTask(id);
            return this.RedirectToAction("AllTasks");
        }

        /// <summary>
        /// The user details.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        public ActionResult UserDetails(int userId)
        {
            this.ViewBag.ManagerName = this.userProcessor.GetUser(userId).UserName;
            this.ViewBag.ManagerId = userId;
            IList<HumanTask> humanTasks = this.taskProcessor.GetTasksList(userId).ToList();
            IList<TaskViewModel> model =
                humanTasks.Select(
                    task =>
                    new TaskViewModel
                    {
                        Task = task,
                        CreatorName =
                            task.CreatorId.HasValue
                                ? this.userProcessor.GetUser(task.CreatorId.Value).UserName
                                : string.Empty
                    }).ToList();

            return this.View(model);
        }

        /// <summary>
        /// The refuse from participate project.
        /// </summary>
        /// <param name="invitationId">
        /// The invitation id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult RefuseFromParticipateProject(int invitationId)
        {
            this.projectProcessor.RefuseFromParticipateProject(invitationId);
            return RedirectToAction("Invitations");
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
                                  ? this.userProcessor.GetUser((int)task.CreatorId).UserName
                                  : "none";
            var assigneeName = task.AssigneeId.HasValue
                                   ? this.userProcessor.GetUser((int)task.AssigneeId).UserName
                                   : "none";
            model.HumanTask = task;
            model.CreatorName = creatorName;
            model.AssigneeName = assigneeName;
            model.TaskHistories = this.taskProcessor.GetAllHistoryForTask(id).OrderByDescending(x => x.ChangeDateTime).ToList();
            return model;
        }
    }
}