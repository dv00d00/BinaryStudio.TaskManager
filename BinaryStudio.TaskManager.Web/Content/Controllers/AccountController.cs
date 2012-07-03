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
        private readonly Logic.Core.IUserRepository userRepository;
        private readonly IEmployeeRepository employeeRepository;
           public AccountController(IUserRepository userRepository, IEmployeeRepository employeeRepository)
           {
               this.userRepository = userRepository;
               this.employeeRepository = employeeRepository;
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
               if (userRepository.LogOn(model.UserName,model.Password))
                {
                    FormsAuthentication.SetAuthCookie(GetCurrentEmployee(model.UserName).Name, false);
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
                return new Employee(){Name = userName + " (Not an Employee)"};
            }
            return employee;
        }
    }
}
