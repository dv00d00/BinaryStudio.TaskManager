namespace BinaryStudio.TaskManager.Web.Controllers
{
    using System.Web.Mvc;
    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;

    using System.Linq;

    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IUserRepository userRepository;

        private readonly IProjectRepository projectRepository;

        public AdminController(IUserRepository userRepository, IProjectRepository projectRepository)
        {
            this.userRepository = userRepository;
            this.projectRepository = projectRepository;
        }

        public ActionResult UsersList()
        {
            return this.View(this.userRepository.GetAll().Where(x=>x.IsDeleted == false));
        }

        public ActionResult EditUser(int userId)
        {
            User user = this.userRepository.GetById(userId);
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
                this.RedirectToAction("AdminPanel");
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
            User user = this.userRepository.GetById(userId);
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
            var user = this.userRepository.GetById(userId);
            user.IsDeleted = true;
            return this.RedirectToAction("AdminPanel");
        }

        public ActionResult AdminPanel()
        {
            return this.View();
        }

        public ActionResult ProjectsList()
        {
            return this.View(this.projectRepository.GetAll());
        }

        public ActionResult EditProject(int projectId)
        {
            var project = this.projectRepository.GetById(projectId);
            return this.View(project);
        }

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
    }
}