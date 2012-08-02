using System.Web.Mvc;

namespace BinaryStudio.TaskManager.Web.Controllers
{
    using BinaryStudio.TaskManager.Logic.Core;

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
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult QuickTaskCreation()
        {
            return this.PartialView("QuickTaskCreation");
        }

        [HttpPost]
        public void QuickTaskCreation(int projectId, string description)
        {
            var creatorId = this.userProcessor.GetUserByName(User.Identity.Name).Id;
            //this.taskProcessor.CreateTask();
        } 
    }
}
