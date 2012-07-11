namespace BinaryStudio.TaskManager.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;

    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;
    using BinaryStudio.TaskManager.Web.Models;

    public class AdminController : Controller
    {
        private readonly IUserRepository userRepository;
                
        public AdminController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;            
        }

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return this.View();
        }


        [Authorize(Roles = "admin")]
        public ActionResult RegisterNewUser()
        {
            return this.View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult RegisterNewUser(RegisterNewUserModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = new User()
                {

                    Id = model.userId,
                    UserName = model.UserName,
                    Email = model.Email,
                    Password = model.Password,
                    RoleId = 2
                };
                this.userRepository.CreateUser(user);

                //TODO: redirect to view with relation employee with account
                return this.RedirectToAction("Index", "Admin");
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }
  
        [Authorize(Roles = "admin")]
        public ActionResult UsersList()
        {
            return this.View(this.userRepository.GetAll());
        }

        [Authorize(Roles = "admin")]
        public ActionResult EditUser(int id)
        {
            User user = this.userRepository.GetById(id);
            return this.View(user);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult EditUser(User user)
        {
            if (this.ModelState.IsValid)
            {
                this.userRepository.Update(user);
                return this.RedirectToAction("Index");
            }
            return this.View(user);
        }

        [Authorize(Roles = "admin")]
        public ActionResult DetailsUser(int id)
        {
            User user= this.userRepository.GetById(id);
            return this.View(user);
        }

        [Authorize(Roles = "admin")]
        public ActionResult DeleteUser(int id)
        {
            User user = this.userRepository.GetById(id);
            return this.View(user);
        }        

        /// <summary>
        /// Deleting account and moving all task to unassigned.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("DeleteUser")]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteUserConfirmed(int id)
        {
            this.userRepository.Delete(id);
            return this.RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult SendUserInfo(int id)
        {
            return this.Json(userRepository.GetById(id));
        }

    }
}