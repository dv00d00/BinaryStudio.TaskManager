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
        private readonly IUserRepository userRepository;
        private readonly IEmployeeRepository employeeRepository;
                
        public AdminController(IUserRepository userRepository, IEmployeeRepository employeeRepository)
        {
            this.userRepository = userRepository;
            this.employeeRepository = employeeRepository;
        }

        public AdminController(IUserRepository userRepository)
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
        public ActionResult RegisterNewUser()
        {
            return View();
        }

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
                                    RoleId = 2
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
            model.CurrentUser = model.Users.First();
            model.CurrentEmployee = model.Employees.First();
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult RegisterNewEmployee()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult RegisterNewEmployee(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employeeRepository.Add(employee);
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        [Authorize(Roles = "admin")]
        public ActionResult EmployeesList()
        {
            return View(employeeRepository.GetAll());
        }

        [Authorize(Roles = "admin")]
        public ActionResult DeleteEmployee(int id)
        {
            Employee employee = employeeRepository.GetById(id);
            return View(employee);
        }

        /// <summary>
        /// Deleting manager and moving all task to unassigned.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("DeleteEmployee")]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteEmployeeConfirmed(int id)
        {
            employeeRepository.Delete(id);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public ActionResult EditEmployee(int id)
        {
            Employee employee = employeeRepository.GetById(id);
            return View(employee);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult EditEmployee(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employeeRepository.Update(employee);
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        [Authorize(Roles = "admin")]
        public ActionResult DetailsEmployee(int id)
        {
            Employee employee = employeeRepository.GetById(id);
            return View(employee);
        }

        [Authorize(Roles = "admin")]
        public ActionResult UsersList()
        {
            return View(userRepository.GetAll());
        }

        [Authorize(Roles = "admin")]
        public ActionResult EditUser(int id)
        {
            User user = userRepository.GetById(id);
            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                userRepository.Update(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [Authorize(Roles = "admin")]
        public ActionResult DetailsUser(int id)
        {
            User user= userRepository.GetById(id);
            return View(user);
        }

        [Authorize(Roles = "admin")]
        public ActionResult DeleteUser(int id)
        {
            User user = userRepository.GetById(id);
            return View(user);
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
            userRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}