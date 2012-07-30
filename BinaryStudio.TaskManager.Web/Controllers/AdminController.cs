namespace BinaryStudio.TaskManager.Web.Controllers
{
    using System.Web.Mvc;
    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;

    /// <summary>
    /// The admin controller for managing application data.
    /// </summary>
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        /// <summary>
        /// The user repository.
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// The project repository.
        /// </summary>
        private readonly IProjectRepository projectRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="userRepository">
        /// The user repository.
        /// </param>
        /// <param name="projectRepository">
        /// The project repository.
        /// </param>
        public AdminController(IUserRepository userRepository, IProjectRepository projectRepository)
        {
            this.userRepository = userRepository;
            this.projectRepository = projectRepository;
        }

        /// <summary>
        /// The users list.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult UsersList()
        {
            return this.View(this.userRepository.GetAll());
        }

        /// <summary>
        /// The edit user.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult EditUser(int userId)
        {
            var user = this.userRepository.GetById(userId);
            return this.View(user);
        }

        /// <summary>
        /// The edit user.
        /// </summary>
        /// <param name="user">
        /// The user, which will be modified
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        public ActionResult EditUser(User user)
        {
            if (this.ModelState.IsValid)
            {
                this.userRepository.UpdateUser(user);
                return this.RedirectToAction("UsersList");
            }

            ModelState.AddModelError(string.Empty, "Wrong data!");
            return this.View(user);
        }

        /// <summary>
        /// The details user.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult DetailsUser(int userId)
        {
            var user = this.userRepository.GetById(userId);
            return this.View(user);
        }

        /// <summary>
        /// The delete user.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult DeleteUser(int userId)
        {
            var user = this.userRepository.GetById(userId);
            return this.View(user);
        }

        /// <summary>
        /// The delete user confirmed.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        [ActionName("DeleteUser")]
        public ActionResult DeleteUserConfirmed(int userId)
        {
            this.userRepository.DeleteUser(userId);
            return this.RedirectToAction("AdminPanel");
        }

        /// <summary>
        /// The admin panel.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult AdminPanel()
        {
            return this.View();
        }

        /// <summary>
        /// The projects list.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult ProjectsList()
        {
            return this.View(this.projectRepository.GetAll());
        }

        /// <summary>
        /// The edit project.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult EditProject(int projectId)
        {
            var project = this.projectRepository.GetById(projectId);
            return this.View(project);
        }

        /// <summary>
        /// The edit project.
        /// </summary>
        /// <param name="project">
        /// The project.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        public ActionResult EditProject(Project project)
        {
            if (this.ModelState.IsValid)
            {
                this.projectRepository.Update(project);
                return this.RedirectToAction("AdminPanel");
            }
            return this.View(project);
        }

        /// <summary>
        /// The details project.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult DetailsProject(int projectId)
        {
            var project = this.projectRepository.GetById(projectId);
            return this.View(project);
        }

        /// <summary>
        /// The delete project.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult DeleteProject(int projectId)
        {
            var project = this.projectRepository.GetById(projectId);
            return this.View(project);
        }

        /// <summary>
        /// The delete project confirmed.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        [ActionName("DeleteProject")]
        public ActionResult DeleteProjectConfirmed(int projectId)
        {
            this.projectRepository.Delete(projectId);
            return this.RedirectToAction("AdminPanel");
        }
    }
}