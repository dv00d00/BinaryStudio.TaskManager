﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HumanTasksController.cs" company="">
//   
// </copyright>
// <summary>
//   Provides access to human task entities
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
    /// Provides access to human task entities
    /// </summary>
    public class HumanTasksController : Controller
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
        /// Initializes a new instance of the <see cref="HumanTasksController"/> class.
        /// </summary>
        /// <param name="taskProcessor">
        /// The task processor.
        /// </param>
        /// <param name="userProcessor">
        /// The user processor. 
        /// </param>
        /// <param name="userRepository">
        /// The user Repository.
        /// </param>
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
        /// The details of current task.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ViewResult.
        /// </returns>
        [Authorize]
        public ViewResult Details(int id)
        {
            var model = this.CreateSingleTaskViewModelById(id);
            return this.View(model);
        }

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="managerId">
        /// The manager id.
        /// </param>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        public ActionResult Create(int managerId, int projectId)
        {
            var createModel = new CreateTaskViewModel
                {
                    Priorities = this.taskProcessor.GetPrioritiesList().OrderBy(x => x.Value),
                    AssigneeId = (managerId != -1) ? managerId : (int?)null,
                    CreatorId = this.userProcessor.GetUserByName(User.Identity.Name).Id,
                    Created = DateTime.Now,
                    ProjectId = projectId,
                };
            return this.View(createModel);
        }

        /// <summary>
        /// Creates the specified human task.
        /// </summary>
        /// <param name="createModel">
        /// The human task.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult Create(CreateTaskViewModel createModel)
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
                                   ProjectId = 1,
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
                return this.RedirectToAction("AllManagersWithTasks");
            }
            createModel.Priorities = taskProcessor.GetPrioritiesList();
            // TODO: refactor this "PossibleCreators" and "PossibleAssignees"
            this.ViewBag.PossibleCreators = new List<User>();
            this.ViewBag.PossibleAssignees = new List<User>();

            return this.View(createModel);
        }

        /// <summary>
        /// The edit human task - GET method.
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
            var model = new SingleTaskViewModel
                            {
                                HumanTask = humantask,
                                Priorities = this.taskProcessor.GetPrioritiesList().OrderBy(x => x.Value)
                            };
            
            this.ViewBag.PossibleCreators = new List<User>();
            this.ViewBag.PossibleAssignees = new List<User>();
            return this.View(model);
        }

        /// <summary>
        /// The edit human task - POST method.
        /// </summary>
        /// <param name="humanTask">
        /// The human task.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult Edit(SingleTaskViewModel model)
        {
            if (this.ModelState.IsValid)
            {

                this.taskProcessor.UpdateTask(model.HumanTask);
                this.taskProcessor.AddHistory(new HumanTaskHistory
                {
                    NewDescription = model.HumanTask.Description,
                    ChangeDateTime = DateTime.Now,
                    NewAssigneeId = model.HumanTask.AssigneeId,
                    NewName = model.HumanTask.Name,
                    Task = model.HumanTask,
                    NewPriority = model.HumanTask.Priority,
                });

                return this.RedirectToAction("AllManagersWithTasks");
            }

            this.ViewBag.PossibleCreators = new List<User>();
            this.ViewBag.PossibleAssignees = new List<User>();
            return this.View(model);
        }

        /// <summary>
        /// The delete human task - GET method.
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
        /// The create single task view createModel by id.
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

        /// <summary>
        /// The delete task is confirmed.
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
            this.ViewBag.ManagerName = this.userRepository.GetById(userId).UserName;
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
                                    ? this.userRepository.GetById(task.CreatorId.Value).UserName
                                    : string.Empty
                        }).ToList();

            return this.View(model);
        }

        /// <summary>
        /// The all managers with tasks.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        public ActionResult AllManagersWithTasks()
        {
            IList<HumanTask> unassignedHumanTasks = this.taskProcessor.GetUnassignedTasks().ToList();
            List<TaskViewModel> unassignedTaskModel =
                unassignedHumanTasks.Select(
                    task =>
                    new TaskViewModel
                    {
                        Task = task,
                        CreatorName = string.Empty
                    }).ToList();
            var model = new ProjectViewModel
                {
                    UsersTasks = new List<ManagerTasksViewModel>(),
                    UnAssignedTasks = unassignedTaskModel
                };
            var users = this.userRepository.GetAll();
            foreach (var user in users)
            {
                IList<HumanTask> humanTasks = this.taskProcessor.GetTasksList(user.Id).ToList();
                List<TaskViewModel> taskModel =
                    humanTasks.Select(
                        task =>
                        new TaskViewModel
                        {
                            Task = task,
                            CreatorName = string.Empty
                        }).ToList();
                var managerModel = new ManagerTasksViewModel();
                managerModel.User = user;
                managerModel.Tasks = taskModel;
                model.UsersTasks.Add(managerModel);
            }
            return this.View(model);
        }

        /// <summary>
        /// The move task between users in project.
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
            if (receiverId == -1)
            {
                this.taskProcessor.MoveTaskToUnassigned(taskId);
            }
        }

    }
}