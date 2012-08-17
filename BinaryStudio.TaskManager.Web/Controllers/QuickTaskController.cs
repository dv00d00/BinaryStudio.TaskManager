// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuickTaskController.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the QuickTaskController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BinaryStudio.TaskManager.Web.Controllers
{
    using System;
    using System.Web.Mvc;

    using BinaryStudio.TaskManager.Extensions.Extentions;
    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;

    /// <summary>
    /// The quick task controller.
    /// </summary>
    public class QuickTaskController : Controller
    {
        /// <summary>
        /// The user processor.
        /// </summary>
        private readonly IUserProcessor userProcessor;

        /// <summary>
        /// The task processor.
        /// </summary>
        private readonly ITaskProcessor taskProcessor;

        /// <summary>
        /// The notifier.
        /// </summary>
        private readonly INotifier notifier;

        /// <summary>
        /// The news processor.
        /// </summary>
        private readonly INewsProcessor newsProcessor;

        /// <summary>
        /// The string extensions.
        /// </summary>
        private readonly IStringExtensions stringExtensions;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickTaskController"/> class.
        /// </summary>
        /// <param name="userProcessor">
        /// The user processor.
        /// </param>
        /// <param name="taskProcessor">
        /// The task processor.
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
        public QuickTaskController(
            IUserProcessor userProcessor,
            ITaskProcessor taskProcessor,
            INotifier notifier,
            INewsProcessor newsProcessor,
            IStringExtensions stringExtensions)
        {
            this.userProcessor = userProcessor;
            this.taskProcessor = taskProcessor;
            this.notifier = notifier;
            this.newsProcessor = newsProcessor;
            this.stringExtensions = stringExtensions;
        }

        /// <summary>
        /// The quick task creation.
        /// </summary>
        /// <param name="humanTask">
        /// The human Task.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult QuickTaskCreation(HumanTask humanTask)
        {
            return this.PartialView("QuickTaskCreation", humanTask);
        }

        /// <summary>
        /// The quick task creation.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        public ActionResult QuickTaskCreation(int projectId, string description)
        {
            var creatorId = this.userProcessor.GetUserByName(User.Identity.Name).Id;

            var task = new HumanTask
            {
                Created = DateTime.Now,
                CreatorId = creatorId,
                Description = description,
                Name = this.stringExtensions.Truncate(description, 15),
                Priority = 0,
                ProjectId = projectId,
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
                UserId = this.userProcessor.GetUserByName(User.Identity.Name).Id
            };
            this.taskProcessor.AddHistory(taskHistory);
            this.newsProcessor.CreateNewsForUsersInProject(taskHistory, task.ProjectId);

            this.notifier.CreateTask(task.Id);

            return this.RedirectToAction("Project", "Project", new { id = task.ProjectId });
        }
    }
}
