using System.Web.Mvc;

namespace BinaryStudio.TaskManager.Web.Controllers
{
    using System;
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
        /// Initializes a new instance of the <see cref="QuickTaskController"/> class.
        /// </summary>
        /// <param name="userProcessor">
        /// The user processor.
        /// </param>
        /// <param name="taskProcessor">
        /// The task Processor.
        /// </param>
        public QuickTaskController(IUserProcessor userProcessor, ITaskProcessor taskProcessor)
        {
            this.userProcessor = userProcessor;
            this.taskProcessor = taskProcessor;
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
            taskName.Append(' ').Append(taskDescription[1]);
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
            return this.RedirectToAction("Project", "Project", new { id = projectId });
        } 
    }
}
