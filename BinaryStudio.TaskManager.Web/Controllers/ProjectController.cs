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

    using BinaryStudio.TaskManager.Extensions.Extentions;
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
        /// The news processor.
        /// </summary>
        private readonly INewsProcessor newsProcessor;

        /// <summary>
        /// The string extensions.
        /// </summary>
        private readonly IStringExtensions stringExtensions;

        /// <summary>
        /// The reminder processor.
        /// </summary>
        private readonly IReminderProcessor reminderProcessor;

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
        /// <param name="newsProcessor">
        /// The news processor.
        /// </param>
        /// <param name="stringExtensions">
        /// The string extensions.
        /// </param>
        /// <param name="reminderProcessor">
        /// The reminder processor.
        /// </param>
        public ProjectController(ITaskProcessor taskProcessor, IUserProcessor userProcessor, IProjectProcessor projectProcessor, 
            INotifier notifier, INewsProcessor newsProcessor, IStringExtensions stringExtensions, IReminderProcessor reminderProcessor)
        {
            this.projectProcessor = projectProcessor;
            this.notifier = notifier;
            this.taskProcessor = taskProcessor;
            this.userProcessor = userProcessor;
            this.newsProcessor = newsProcessor;
            this.stringExtensions = stringExtensions;
            this.reminderProcessor = reminderProcessor;
        }

        /// <summary>
        /// The project.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="isOpenedProjects">
        /// The is opened projects.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult Project(int id, int? userId, bool isOpenedProjects = true)
        {
            IList<HumanTask> unassignedHumanTasks = this.taskProcessor.GetUnAssignedTasksForProject(id).ToList();
            List<TaskViewModel> unassignedTaskModel =
                unassignedHumanTasks.Select(
                    task =>
                    new TaskViewModel
                    {
                        Task = task,
                        TaskName = this.stringExtensions.Truncate(task.Name, 15),
                        CreatorName = string.Empty
                    }).ToList();
            var model = new ProjectViewModel
            {
                UsersTasks = new List<ManagerTasksViewModel>(),
                UnAssignedTasks = unassignedTaskModel,
                QuickTask = new HumanTask(),
                ProjectId = id,
                ChosenUserId = userId,
                ChosenUserTasks = new ManagerTasksViewModel(),
                NumberOfUsers = this.projectProcessor.GetUsersAndCreatorInProject(id).Count()
            };
            model.QuickTask.ProjectId = id;

            var users = new List<User>();
            users = this.projectProcessor.GetUsersAndCreatorInProject(id).Reverse().ToList();
            foreach (var user in users)
            {
                IList<HumanTask> humanTasks = isOpenedProjects
                        ? this.taskProcessor.GetAllOpenTasksForUserInProject(id, user.Id).ToList()
                        : this.taskProcessor.GetAllClosedTasksForUserInProject(id, user.Id).ToList();
                List<TaskViewModel> taskModel =
                    humanTasks.Select(
                        task =>
                        new TaskViewModel
                        {
                            Task = task,
                            TaskName = this.stringExtensions.Truncate(task.Name, 15),
                            CreatorName = string.Empty
                        }).ToList();
                var managerModel = new ManagerTasksViewModel
                {
                    User = user,
                    ProjectId = id,
                    Tasks = taskModel
                };
                model.UsersTasks.Add(managerModel);
            }

            if (userId != null)
            {
                IList<HumanTask> chosenTasks = isOpenedProjects
                         ? this.taskProcessor.GetAllOpenTasksForUserInProject(id, (int)userId).ToList()
                         : this.taskProcessor.GetAllClosedTasksForUserInProject(id, (int)userId).ToList();
                List<TaskViewModel> chosenTaskModel =
                    chosenTasks.Select(
                        task =>
                        new TaskViewModel
                        {
                            Task = task,
                            TaskName = this.stringExtensions.Truncate(task.Name, 15),
                            CreatorName = string.Empty,
                            ViewStyle = false
                        }).ToList();
                model.ChosenUserTasks.User = this.userProcessor.GetUser((int)userId);
                model.ChosenUserTasks.ProjectId = id;
                model.ChosenUserTasks.Tasks = chosenTaskModel;                
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
        /// <param name="viewStyle">
        /// The view Style.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult CreateTask(int userId, int projectId, bool? viewStyle)
        {
            var createModel = new CreateTaskViewModel
            {
                Priorities = this.taskProcessor.GetPrioritiesList().OrderBy(x => x.Value),
                AssigneeId = (userId != -1) ? userId : (int?)null,
                CreatorId = this.userProcessor.GetUserByName(User.Identity.Name).Id,
                Created = DateTime.Now,
                Tasks = this.taskProcessor.GetOpenTasksListInProject(projectId),
                ProjectId = projectId,
                ViewStyle = viewStyle
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
                    BlockingTaskId = createModel.BlockingTask
                };

                if (task.Priority == 3)
                {
                    task.AssigneeId = this.userProcessor.GetUserByTaskId(task.BlockingTaskId);
                    task.Assigned = task.AssigneeId == (int?)null ? task.Created : (DateTime?)null;
                }

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
                                          UserId = this.userProcessor.GetUserByName(User.Identity.Name).Id
                                      };
                this.taskProcessor.AddHistory(taskHistory);
                this.notifier.CreateTask(task.Id);
                this.newsProcessor.CreateNewsForUsersInProject(taskHistory, task.ProjectId);

                if (true == createModel.ViewStyle)
                {
                    return this.RedirectToAction("MultiuserView", new { projectId = createModel.ProjectId, userId = createModel.AssigneeId });
                }

                return this.RedirectToAction("Project", new { id = createModel.ProjectId, userId = createModel.AssigneeId });
            }

            createModel.Priorities = taskProcessor.GetPrioritiesList();
            // TODO: refactor this "PossibleCreators" and "PossibleAssignees"
            this.ViewBag.PossibleCreators = new List<User>();
            this.ViewBag.PossibleAssignees = new List<User>();

            return this.View(createModel);
        }
        
        /// <summary>
        /// Moves task from one user to another.
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
                UserId = this.userProcessor.GetUserByName(User.Identity.Name).Id,
                NewDescription = humanTask.Description,
                NewPriority = humanTask.Priority,
                NewName = humanTask.Name,
                Task = humanTask,
                TaskId = taskId
            };
            taskProcessor.AddHistory(humanTaskHistory);
            this.newsProcessor.CreateNewsForUsersInProject(humanTaskHistory,humanTask.ProjectId);


            if (receiverId != -1)
            {
                this.taskProcessor.MoveTask(taskId, receiverId);
            }
            else
            {
                this.taskProcessor.MoveTaskToUnassigned(taskId);
            }
            
            this.notifier.MoveTask(humanTask, receiverId);
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
        /// <param name="viewStyle">
        /// The view Style.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        public ActionResult Edit(int taskId, int projectId, bool? viewStyle)
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
                Tasks = this.taskProcessor.GetOpenTasksListInProject(projectId),
                ProjectId = projectId,
                Id = taskId,
                ViewStyle = viewStyle
            };
            return this.View(createModel);
        }

        /// <summary>
        /// Task edit method.
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
                                          UserId = this.userProcessor.GetUserByName(User.Identity.Name).Id
                                      };
                
                this.taskProcessor.AddHistory(taskHistory);
                this.newsProcessor.CreateNewsForUsersInProject(taskHistory, humanTask.ProjectId);

                if (true == createModel.ViewStyle)
                {
                    return this.RedirectToAction("MultiuserView", new { projectId = createModel.ProjectId, userId = createModel.AssigneeId });
                }

                return this.RedirectToAction("Project", new { id = createModel.ProjectId, userId = createModel.AssigneeId });
            }

            return this.View(createModel);
        }

        /// <summary>
        /// The details.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="viewStyle">
        /// The view Style.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult Details(int id, bool? viewStyle)
        {
            var model = this.CreateSingleTaskViewModelById(id, viewStyle);            
            return this.View(model);
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="idTask">
        /// The id Task.
        /// </param>
        /// <param name="viewStyle">
        /// The view Style.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        public ActionResult Delete(int idTask, bool? viewStyle)
        {
            var model = this.CreateSingleTaskViewModelById(idTask, viewStyle);
            return this.View(model);
        }

        /// <summary>
        /// The delete confirmed.
        /// </summary>
        /// <param name="idTask">
        /// The id Task.
        /// </param>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <param name="viewStyle">
        /// The view Style.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        //[ActionName("Delete")]
        public ActionResult DeleteConfirmed(int idTask, int projectId, bool? viewStyle)
        {
            //var userId = this.taskProcessor.GetTaskById(idTask).AssigneeId;
            this.taskProcessor.DeleteTask(idTask);
            
            if (true == viewStyle)
            {
                //return this.RedirectToAction("MultiuserView", new { projectId, userId });
            }

            return this.RedirectToAction("Project", new { id = projectId });
        }
        
        /// <summary>
        /// The create single task view model by id.
        /// </summary>
        /// <param name="taskId">
        /// The task id.
        /// </param>
        /// <param name="viewStyle">
        /// The view Style.
        /// </param>
        /// <returns>
        /// The BinaryStudio.TaskManager.Web.Models.SingleTaskViewModel.
        /// </returns>
        private SingleTaskViewModel CreateSingleTaskViewModelById(int taskId, bool? viewStyle)
        {
            var task = this.taskProcessor.GetTaskById(taskId);
            var creatorName = task.CreatorId.HasValue
                                  ? this.userProcessor.GetUser((int)task.CreatorId).UserName
                                  : "none";
            var assigneeName = task.AssigneeId.HasValue
                                   ? this.userProcessor.GetUser((int)task.AssigneeId).UserName
                                   : "none";
            var blockedTaskName = "none";
            if (task.BlockingTaskId != 0)
            {
                blockedTaskName = this.taskProcessor.GetTaskById(task.BlockingTaskId).Name;
            }

            var model = new SingleTaskViewModel
                            {
                                HumanTask = task,
                                CreatorName = creatorName,
                                AssigneeName = assigneeName,
                                TaskHistories =
                                    this.taskProcessor.GetAllHistoryForTask(taskId).OrderByDescending(
                                        x => x.ChangeDateTime)
                                    .ToList(),
                                Priorities = this.taskProcessor.GetPrioritiesList(),
                                BlockedTaskName = blockedTaskName,
                                ViewStyle = viewStyle
                            };
            return model;
        }

        /// <summary>
        /// The task view.
        /// </summary>
        /// <param name="taskId">
        /// The task id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        public ActionResult TaskView(int taskId)
        {
            var task = taskProcessor.GetTaskById(taskId);
            task.Name = stringExtensions.Truncate(task.Name, 15);
            task.Description = stringExtensions.Truncate(task.Description, 50);
            var model = new TaskViewModel { Task = task, TaskName = this.stringExtensions.Truncate(task.Name, 15), CreatorName = string.Empty };
            return this.PartialView("ManagerTasksTablePartialView", model);
        }

        /// <summary>
        /// The make task close.
        /// </summary>
        /// <param name="taskId">
        /// The task id.
        /// </param>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        public void MakeTaskClose(int taskId, int projectId)
        {
            this.taskProcessor.CloseTask(taskId);
            HumanTask humanTask = this.taskProcessor.GetTaskById(taskId);
            HumanTaskHistory humanTaskHistory = new HumanTaskHistory
            {
                Action = ChangeHistoryTypes.Close,
                ChangeDateTime = DateTime.Now,
                NewAssigneeId = humanTask.AssigneeId,
                UserId = this.userProcessor.GetUserByName(User.Identity.Name).Id,
                NewDescription = humanTask.Description,
                NewPriority = humanTask.Priority,
                NewName = humanTask.Name,
                Task = humanTask,
                TaskId = taskId
            };

            taskProcessor.AddHistory(humanTaskHistory);
            this.newsProcessor.CreateNewsForUsersInProject(humanTaskHistory, humanTask.ProjectId);
           
        }

        /// <summary>
        /// The multyuser view.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="isOpenedProjects">
        /// The is opened projects.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult MultiuserView(int projectId, int? userId, bool isOpenedProjects = true)
        {
            IList<HumanTask> unassignedHumanTasks = this.taskProcessor.GetUnAssignedTasksForProject(projectId).ToList();
            List<TaskViewModel> unassignedTaskModel =
                unassignedHumanTasks.Select(
                    task =>
                    new TaskViewModel
                    {
                        Task = task,
                        TaskName = this.stringExtensions.Truncate(task.Name, 15),
                        CreatorName = string.Empty,
                        ViewStyle = true
                    }).ToList();
            var model = new ProjectViewModel
            {
                UsersTasks = new List<ManagerTasksViewModel>(),
                UnAssignedTasks = unassignedTaskModel,
                QuickTask = new HumanTask(),
                ProjectId = projectId,
                ChosenUserId = userId,
                ChosenUserTasks = new ManagerTasksViewModel(),
                NumberOfUsers = this.projectProcessor.GetUsersAndCreatorInProject(projectId).Count()
            };
            model.QuickTask.ProjectId = projectId;

            var users = new List<User>();
            users = this.projectProcessor.GetUsersAndCreatorInProject(projectId).Reverse().ToList();
            foreach (var user in users)
            {
                IList<HumanTask> humanTasks = isOpenedProjects
                        ? this.taskProcessor.GetAllOpenTasksForUserInProject(projectId, user.Id).ToList()
                        : this.taskProcessor.GetAllClosedTasksForUserInProject(projectId, user.Id).ToList();
                List<TaskViewModel> taskModel =
                    humanTasks.Select(
                        task =>
                        new TaskViewModel
                        {
                            Task = task,
                            TaskName = this.stringExtensions.Truncate(task.Name, 15),
                            CreatorName = string.Empty
                        }).ToList();
                var managerModel = new ManagerTasksViewModel
                {
                    User = user,
                    ProjectId = projectId,
                    Tasks = taskModel
                };
                model.UsersTasks.Add(managerModel);
            }

            if (userId != null)
            {
                IList<HumanTask> chosenTasks = isOpenedProjects
                         ? this.taskProcessor.GetAllOpenTasksForUserInProject(projectId, (int)userId).ToList()
                         : this.taskProcessor.GetAllClosedTasksForUserInProject(projectId, (int)userId).ToList();
                List<TaskViewModel> chosenTaskModel =
                    chosenTasks.Select(
                        task =>
                        new TaskViewModel
                        {
                            Task = task,
                            TaskName = this.stringExtensions.Truncate(task.Name, 15),
                            CreatorName = string.Empty,
                            ViewStyle = true
                        }).ToList();
                model.ChosenUserTasks.User = this.userProcessor.GetUser((int)userId);
                model.ChosenUserTasks.ProjectId = projectId;
                model.ChosenUserTasks.Tasks = chosenTaskModel;
            }

            return this.View(model);
        }
    }
}