using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using BinaryStudio.TaskManager.Logic.Core;
using BinaryStudio.TaskManager.Logic.Domain;
using BinaryStudio.TaskManager.Web.Models;

namespace BinaryStudio.TaskManager.Web.Content.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserRepository userRepository;
        private readonly IEmployeeRepository employeeRepository;
        //
        // GET: /User/Register
        public AdminController(UserRepository userRepository, IEmployeeRepository employeeRepository)
        {
            this.userRepository = userRepository;
            this.employeeRepository = employeeRepository;
        }

        public AdminController(UserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.employeeRepository = new EmployeeRepository(new DataBaseContext());
        }

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View();
        }


        [Authorize(Roles = "admin")]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult RegisterNewUser(RegisterNewUserModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User()
                                {
                                    Id = model.userId, 
                                    UserName = model.UserName, 
                                    Email = model.Email, 
                                    Password = model.Password,
                                    RoleId = 1
                                };
                userRepository.CreateUser(user);
                
                //FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                //TODO: redirect to view with relation employee with account
                return RedirectToAction("ConnectUserWithEmployee", "Admin");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // GET: /Account/ChangePassword
        [Authorize(Roles = "admin")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword   
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "Неверен текущий пароль или новый пароль с ошибками.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess
        [Authorize(Roles = "admin")]
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult ConnectUserWithEmployee()
        {
            var model = new UserViewModel();
            model.Users = userRepository.GetAll().ToList();
            model.Employees = employeeRepository.GetAll().ToList();
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public ActionResult RegisterNewEmployee()
        {
            throw new NotImplementedException();
        }
    }
}