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
<<<<<<< HEAD
        /// The project repository.
        /// </summary>
        private readonly IProjectRepository projectRepository;

        /// <summary>
=======
>>>>>>> 715b24ea12f2d4451d1f27b55174f928386b2726
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
<<<<<<< HEAD
        /// <param name="projectRepository">
        /// The project repository.
        /// </param>
        public ProjectController(ITaskProcessor taskProcessor, IUserProcessor userProcessor, IUserRepository userRepository, IProjectRepository projectRepository)
=======
        public ProjectController(ITaskProcessor taskProcessor, IUserProcessor userProcessor, IProjectProcessor projectProcessor)
>>>>>>> 715b24ea12f2d4451d1f27b55174f928386b2726
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
<<<<<<< HEAD
            var humanTask = new HumanTask();
            humanTask.AssigneeId = (userId != -1) ? userId : (int?)null;
            humanTask.CreatorId = this.userProcessor.GetCurrentLoginedUser(User.Identity.Name).Id;
            humanTask.Created = DateTime.Now;
=======
            var humanTask = new HumanTask
                {
                    AssigneeId = (userId != -1) ? userId : (int?)null,
                    CreatorId = this.userProcessor.GetUserByName(this.User.Identity.Name).Id,
                    Created = DateTime.Now
                };
>>>>>>> 715b24ea12f2d4451d1f27b55174f928386b2726
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
            const int ProjectId = 1;
            var model = new ProjectViewModel
                {
                    UsersTasks = new List<ManagerTasksViewModel>(),
                    UnAssignedTasks = this.taskProcessor.GetUnassignedTasks().ToList()
                };
<<<<<<< HEAD
            var users = this.projectRepository.GetAllUsersInProject(projectId);
=======
            var users = this.projectProcessor.GetAllUsersInProject(ProjectId);
>>>>>>> 715b24ea12f2d4451d1f27b55174f928386b2726
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
            return this.View(this.userProcessor.GetAllUsers());
        }

        /// <summary>
<<<<<<< HEAD
        /// The add user.
=======
        /// The invite user in project.
>>>>>>> 715b24ea12f2d4451d1f27b55174f928386b2726
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
<<<<<<< HEAD
        public ActionResult AddUser(int userId, int projectId)
        {
            var user = this.userRepository.GetById(userId);
            user.UserProjects.Add(this.projectRepository.GetById(projectId));
            this.userRepository.UpdateUser(user);

            var project = this.projectRepository.GetById(projectId);
            project.ProjectUsers.Add(this.userRepository.GetById(userId));
            this.projectRepository.Update(project);
            
=======
        public ActionResult InviteUserInProject(int userId, int projectId)
        {
            this.projectProcessor.InviteUserInProject(userId, projectId);                        
>>>>>>> 715b24ea12f2d4451d1f27b55174f928386b2726
            return this.RedirectToAction("PersonalProject");
        }

        /// <summary>
<<<<<<< HEAD
        /// The delete user.
=======
        /// The remove user from project.
>>>>>>> 715b24ea12f2d4451d1f27b55174f928386b2726
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
<<<<<<< HEAD
        public ActionResult DeleteUser(int userId, int projectId)
        {
            var user = this.userRepository.GetById(userId);
            user.UserProjects.Remove(this.projectRepository.GetById(projectId));
            this.userRepository.UpdateUser(user);

            var project = this.projectRepository.GetById(projectId);
            project.ProjectUsers.Remove(this.userRepository.GetById(userId));
            this.projectRepository.Update(project);
=======
        public ActionResult RemoveUserFromProject(int userId, int projectId)
        {
            this.projectProcessor.RemoveUserFromProject(userId, projectId);
            return this.RedirectToAction("PersonalProject");
        }

        public ActionResult Invitations()
        {
            var model = new List<InvitationsViewModel>();
            var user = this.userProcessor.GetUserByName(User.Identity.Name);
            var invitationsToUser = this.projectProcessor.GetAllInvitationsToUser(user.Id);
>>>>>>> 715b24ea12f2d4451d1f27b55174f928386b2726

            return this.View();
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
        /// The edit.
        /// </summary>
        /// <param name="humanTask">
        /// The human task.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            this.taskProcessor.DeleteTask(id);
            return this.RedirectToAction("AllTasks");
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