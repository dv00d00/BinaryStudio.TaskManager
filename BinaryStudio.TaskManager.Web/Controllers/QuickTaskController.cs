using System.Web.Mvc;

namespace BinaryStudio.TaskManager.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;
    using BinaryStudio.TaskManager.Web.Models;

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
        /// Initializes a new instance of the <see cref="QuickTaskController"/> class.
        /// </summary>
        /// <param name="userProcessor">
        /// The user processor.
        /// </param>
        /// <param name="taskProcessor">
        /// The task Processor.
        /// </param>
        /// <param name="projectProcessor">
        /// The project Processor.
        /// </param>
        /// <param name="notifier">
        /// The notifier.
        /// </param>
        /// <param name="newsRepository">
        /// The news Repository.
        /// </param>
        public QuickTaskController(IUserProcessor userProcessor, ITaskProcessor taskProcessor, IProjectProcessor projectProcessor, INotifier notifier, INewsRepository newsRepository)
        {
            this.userProcessor = userProcessor;
            this.taskProcessor = taskProcessor;
            this.projectProcessor = projectProcessor;
            this.notifier = notifier;
            this.newsRepository = newsRepository;
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
            var separetor = new[] { ' ' };
            var taskDescription = description.Split(separetor, 3);
            var taskName = new StringBuilder(taskDescription[0], 20);
            if (taskDescription.Length > 1)
            {
                taskName.Append(' ').Append(taskDescription[1]);
            }

            var task = new HumanTask
            {
                Created = DateTime.Now,
                CreatorId = creatorId,
                Description = description,
                Name = taskName.ToString(),
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

            List<User> projectUsers = new List<User>(this.projectProcessor.GetAllUsersInProject(task.ProjectId));
            projectUsers.Add(this.projectProcessor.GetProjectById(task.ProjectId).Creator);

            this.CreateNewsForUsers(taskHistory, projectUsers);

            this.notifier.CreateTask(task.Id);

            return this.RedirectToAction("Project", "Project", new { id = task.ProjectId });
        }

        /// <summary>
        /// The create news for users.
        /// </summary>
        /// <param name="taskHistory">
        /// The task history.
        /// </param>
        /// <param name="projectUsers">
        /// The project users.
        /// </param>
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

                this.newsRepository.AddNews(news);
                this.notifier.SetCountOfNewses(projectUser.UserName);
            }
        }
    }
}
