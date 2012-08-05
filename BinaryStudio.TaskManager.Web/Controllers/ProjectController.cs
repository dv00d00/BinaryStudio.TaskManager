using System.Globalization;

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
        /// The notifier.
        /// </summary>
        private readonly INotifier notifier;

        /// <summary>
        /// The news repository
        /// </summary>
        private readonly INewsRepository newsRepository;

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
        /// <param name="notifier">
        /// The notifier.
        /// </param>
        public ProjectController(ITaskProcessor taskProcessor, IUserProcessor userProcessor, IProjectProcessor projectProcessor, INotifier notifier, INewsRepository newsRepository)
        {
            this.projectProcessor = projectProcessor;
            this.notifier = notifier;
            this.taskProcessor = taskProcessor;
            this.userProcessor = userProcessor;
            this.newsRepository = newsRepository;
        }

        /// <summary>
        /// The project.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="isOpenedProjects">
        /// The is Opened Projects.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult Project(int id, bool isOpenedProjects = true)
        {
            var model = new ProjectViewModel
            {
                UsersTasks = new List<ManagerTasksViewModel>(),
                UnAssignedTasks = this.taskProcessor.GetUnAssignedTasksForProject(id).ToList(),
                QuickTask = new HumanTask(),
                ProjectId = id
            };
            model.QuickTask.ProjectId = id;
            var users = new List<User>();
            users = this.projectProcessor.GetAllUsersInProject(id).Reverse().ToList();
            users.Add(this.projectProcessor.GetProjectById(id).Creator);
            foreach (var user in users)
            {
                var managerModel = new ManagerTasksViewModel
                {
                    User = user,
                    Tasks = isOpenedProjects
                        ? this.taskProcessor.GetAllOpenTasksForUserInProject(id, user.Id).ToList()
                        : this.taskProcessor.GetAllClosedTasksForUserInProject(id, user.Id).ToList()
                };
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
        /// <param name="projectId">
        /// The project id.
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
                IsBlocking = false,
                Tasks = this.taskProcessor.GetTasksInProjectList(projectId),
                ProjectId = projectId
            };
            createModel.Priority = int.Parse(createModel.Priorities.First().Value);
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
                var taskHistory = new HumanTaskHistory
                                      {
                                          NewDescription = task.Description,
                                          ChangeDateTime = DateTime.Now,
                                          NewAssigneeId = task.AssigneeId,
                                          NewName = task.Name,
                                          Task = task,
                                          NewPriority = task.Priority,
                                          Action = ChangeHistoryTypes.Create,
                                          UserId = userProcessor.GetUserByName(User.Identity.Name).Id
                                      };
                this.taskProcessor.AddHistory(taskHistory);

                List<User> projectUsers = new List<User>(projectProcessor.GetAllUsersInProject(createModel.ProjectId));
                projectUsers.Add(this.projectProcessor.GetProjectById(createModel.ProjectId).Creator);

                CreateNewsForUsers(taskHistory, projectUsers);

                notifier.CreateTask(task.Id);

                return this.RedirectToAction("Project", new { id = createModel.ProjectId });
            }
            createModel.Priorities = taskProcessor.GetPrioritiesList();
            // TODO: refactor this "PossibleCreators" and "PossibleAssignees"
            this.ViewBag.PossibleCreators = new List<User>();
            this.ViewBag.PossibleAssignees = new List<User>();

            return this.View(createModel);
        }

        private void CreateNewsForUsers(HumanTaskHistory taskHistory, IEnumerable<User> projectUsers)
        {
            foreach (var projectUser in projectUsers)
            {
                var news = new News
                               {
                                   HumanTaskHistory = taskHistory,
                                   IsRead = false,
                                   User = projectUser,
                                   UserId = projectUser.Id,
                                   HumanTaskHistoryId = taskHistory.Id,
                               };

                newsRepository.AddNews(news);
                notifier.SetCountOfNewses(projectUser.UserName);
            }
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
        /// <param name="projectId"> </param>
        [Authorize]
        public void MoveTask(int taskId, int senderId, int receiverId, int projectId)
        {
            
            HumanTask humanTask = taskProcessor.GetTaskById(taskId);
            HumanTaskHistory humanTaskHistory = new HumanTaskHistory
            {
                Action = ChangeHistoryTypes.Move,
                ChangeDateTime = DateTime.Now,
                NewAssigneeId = receiverId == -1 ? (int?)null : receiverId,
                //???????????????????
                UserId = userProcessor.GetUserByName(User.Identity.Name).Id,
                NewDescription = humanTask.Description,
                NewPriority = humanTask.Priority,
                NewName = humanTask.Name,
                Task = humanTask,
                TaskId = taskId
            };
            taskProcessor.AddHistory(humanTaskHistory);
            List<User> users = new List<User>(projectProcessor.GetAllUsersInProject(projectId));
            users.Add(projectProcessor.GetProjectById(projectId).Creator);
            CreateNewsForUsers(humanTaskHistory, users);
            if (receiverId != -1)
            {
                this.taskProcessor.MoveTask(taskId, receiverId);
            }
            else
            {
                this.taskProcessor.MoveTaskToUnassigned(taskId);
            }
            
            this.notifier.MoveTask(taskId, receiverId);
            
        }

        /// <summary>
        /// The invite or delete user.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult InviteOrDeleteUser(int projectId)
        {
            var currentUser = this.userProcessor.GetUserByName(User.Identity.Name);
            var listWithCurrentUser = new List<User> { currentUser };
            var users = this.userProcessor.GetAllUsers();
            users = users.Except(listWithCurrentUser);

            var invitationsToProject = this.projectProcessor.GetAllInvitationsToProject(projectId).Where(x => x.IsInvitationConfirmed == false && x.Sender == currentUser);

            //var invitationsToProject = this.projectProcessor.GetAllInvitationsToProject(ProjectId).Where(x => x.IsInvitationConfirmed == false);

            var listAlreadyInvited = invitationsToProject.Select(invitation => invitation.Receiver).ToList();

            var collaborators = this.projectProcessor.GetAllUsersInProject(projectId);
            var listToInvite = users.Except(collaborators).Except(listAlreadyInvited);
            var model = new ProjectCollaboratorsViewModel { Collaborators = collaborators, PossibleCollaborators = listToInvite, AlreadyInvited = listAlreadyInvited, ProjectId = projectId };
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
        [HttpPost]
        public void InviteUserInProject(int receiverId, int projectId)
        {
            var senderId = this.userProcessor.GetUserByName(User.Identity.Name).Id;
            this.projectProcessor.InviteUserInProject(senderId, projectId, receiverId);
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
        [HttpPost]
        public void RemoveUserFromProject(int userId, int projectId)
        {
            this.projectProcessor.RemoveUserFromProject(userId, projectId);
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
        [HttpPost]
        public void SubmitInvitationInProject(int invitationId)
        {
            this.projectProcessor.ConfirmInvitationInProject(invitationId);
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
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ViewResult.
        /// </returns>
        [Authorize]
        public ViewResult AllTasks(int projectId)
        {
            var model = new List<SingleTaskViewModel>();
            string creatorName, assigneeName;
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

            return this.View(model);
        }

        /// <summary>
        /// The edit.
        /// </summary>
        /// <param name="taskId">
        /// The id.
        /// </param>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        public ActionResult Edit(int taskId, int projectId)
        {
            var task = this.taskProcessor.GetTaskById(taskId);
            var createModel = new CreateTaskViewModel
            {
                Priorities = this.taskProcessor.GetPrioritiesList().OrderBy(x => x.Value),
                Assigned = task.Assigned,
                AssigneeId = task.AssigneeId,
                Created = task.Created,
                Priority = task.Priority,
                CreatorId = task.CreatorId,
                Description = task.Description,
                Name = task.Name,
                Finished = task.Finished,
                ProjectId = projectId,
                Id = taskId
            };
            return this.View(createModel);
        }

        /// <summary>
        /// The edit.
        /// </summary>
        /// <param name="createModel">
        /// The human task.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        public ActionResult Edit(CreateTaskViewModel createModel)
        {
            if (this.ModelState.IsValid)
            {
                var humanTask = this.taskProcessor.GetTaskById(createModel.Id);

                humanTask.Name = createModel.Name;
                humanTask.Priority = createModel.Priority;
                humanTask.Finished = createModel.Finished;
                humanTask.Description = createModel.Description;
                this.taskProcessor.UpdateTask(humanTask);
                var taskHistory = new HumanTaskHistory
                                      {
                                          NewDescription = createModel.Description,
                                          ChangeDateTime = DateTime.Now,
                                          NewAssigneeId = createModel.AssigneeId,
                                          NewName = createModel.Name,
                                          Task = humanTask,
                                          TaskId = humanTask.Id,
                                          NewPriority = createModel.Priority,
                                          Action = ChangeHistoryTypes.Change,
                                          UserId = userProcessor.GetUserByName(User.Identity.Name).Id
                                      };
                var usersInProject = new List<User>(projectProcessor.GetAllUsersInProject(createModel.ProjectId));
                usersInProject.Add(humanTask.Project.Creator);
                this.taskProcessor.AddHistory(taskHistory);
                CreateNewsForUsers(taskHistory, usersInProject);
                return this.RedirectToAction("Project", new { id = createModel.ProjectId });
            }

            return this.View(createModel);
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
        public ActionResult Delete(int idTask)
        {
            var model = this.CreateSingleTaskViewModelById(idTask);
            return this.View(model);
        }

        /// <summary>
        /// The delete confirmed.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int idTask, int projectId)
        {
            this.taskProcessor.DeleteTask(idTask);
            return this.RedirectToAction("Project", new { id = projectId });
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
        [HttpPost]
        public void RefuseFromParticipateProject(int invitationId)
        {
            this.projectProcessor.RefuseFromParticipateProject(invitationId);
        }

        /// <summary>
        /// The create single task view model by id.
        /// </summary>
        /// <param name="taskId">
        /// The task id.
        /// </param>
        /// <returns>
        /// The BinaryStudio.TaskManager.Web.Models.SingleTaskViewModel.
        /// </returns>
        private SingleTaskViewModel CreateSingleTaskViewModelById(int taskId)
        {

            var task = this.taskProcessor.GetTaskById(taskId);
            var creatorName = task.CreatorId.HasValue
                                  ? this.userProcessor.GetUser((int)task.CreatorId).UserName
                                  : "none";
            var assigneeName = task.AssigneeId.HasValue
                                   ? this.userProcessor.GetUser((int)task.AssigneeId).UserName
                                   : "none";
            var model = new SingleTaskViewModel
                            {
                                HumanTask = task,
                                CreatorName = creatorName,
                                AssigneeName = assigneeName,
                                TaskHistories =
                                    this.taskProcessor.GetAllHistoryForTask(taskId).OrderByDescending(
                                        x => x.ChangeDateTime)
                                    .ToList(),
                                Priorities = this.taskProcessor.GetPrioritiesList()
                            };
            return model;
        }





        [HttpPost]
        public ActionResult TaskView(int taskId)
        {
            var task = taskProcessor.GetTaskById(taskId);
            return PartialView("ManagerTasksTablePartialView", task);
        }

        public void MakeTaskClose(int taskId, int projectId)
        {
            taskProcessor.CloseTask(taskId);
            HumanTask humanTask = taskProcessor.GetTaskById(taskId);
            HumanTaskHistory humanTaskHistory = new HumanTaskHistory
            {
                Action = ChangeHistoryTypes.Close,
                ChangeDateTime = DateTime.Now,
                NewAssigneeId = humanTask.AssigneeId,
                UserId = userProcessor.GetUserByName(User.Identity.Name).Id,
                NewDescription = humanTask.Description,
                NewPriority = humanTask.Priority,
                NewName = humanTask.Name,
                Task = humanTask,
                TaskId = taskId
            };
            taskProcessor.AddHistory(humanTaskHistory);
            List<User> users = new List<User>(projectProcessor.GetAllUsersInProject(projectId));
            users.Add(projectProcessor.GetProjectById(projectId).Creator);
            CreateNewsForUsers(humanTaskHistory, users);
        }
    }
}