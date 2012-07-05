using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using BinaryStudio.TaskManager.Logic.Core;
using BinaryStudio.TaskManager.Logic.Domain;
using BinaryStudio.TaskManager.Web.Models;

namespace BinaryStudio.TaskManager.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IUserProcessor userProcessor;
        public AccountController(IUserRepository userRepository, IEmployeeRepository employeeRepository, IUserProcessor userProcessor)
        {
            this.userRepository = userRepository;
            this.employeeRepository = employeeRepository;
            this.userProcessor = userProcessor;
        }

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }
        
        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (userProcessor.LogOnUser(model.UserName, model.Password))
                {
                    userProcessor.SetRoleToUserFromDB(model.UserName);
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Введенное имя пользователя или пароль неверны.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //TODO: refactor!!!!!!!!!!!!!!!!!!!!!!!!!!
        Employee GetCurrentEmployee(string userName)
        {
            User user = userRepository.GetByName(userName);
            Employee employee = new Employee();
            try
            {
                employee = employeeRepository.GetAll().ToList().Single(it => it.UserId == user.Id);
            }
            catch (Exception)
            {
                return new Employee() { Name = "(Not an Employee)" };
            }
            return employee;
        }

        public void SetUserRoleFromBase(string userName)
        {
            if (!Roles.RoleExists(userRepository.GetRoleByName(userName)))
            {
                Roles.CreateRole(userRepository.GetRoleByName(userName));
            }

            if (!Roles.IsUserInRole(userName, userRepository.GetRoleByName(userName)))
            {
                Roles.AddUserToRole(userName, userRepository.GetRoleByName(userName));
            }
        }
    }
}
