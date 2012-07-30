namespace BinaryStudio.TaskManager.Web.Controllers
{
    using System.Web.Mvc;
    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;

    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IUserRepository userRepository;

        public AdminController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public ActionResult UsersList()
        {
            return this.View(this.userRepository.GetAll());
        }

        public ActionResult EditUser(int id)
        {
            User user = this.userRepository.GetById(id);
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
                return this.RedirectToRoute("Default", null);
            }
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
            User user = this.userRepository.GetById(userId);
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
            this.userRepository.DeleteUser(userId);
            return this.RedirectToRoute("Default", null);
        }

        public ActionResult AdminPanel()
        {
            throw new System.NotImplementedException();
        }
    }
}