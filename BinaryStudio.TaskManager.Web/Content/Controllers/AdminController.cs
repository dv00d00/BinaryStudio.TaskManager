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

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterNewUserModel model)
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
        
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword        
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

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }


        public ActionResult ConnectUserWithEmployee()
        {
            var model = new UserViewModel();            
            return View(model);
        }
    }
}