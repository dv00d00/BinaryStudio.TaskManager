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
                User user = new User()
                                {

                                    Id = model.userId,
                                    UserName = model.UserName,
                                    Email = model.Email,
                                    Password = model.Password,
                                    RoleId = 2
                                };
                this.userRepository.CreateUser(user);

                //FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                //TODO: redirect to view with relation employee with account
                return this.RedirectToAction("ConnectUserWithEmployee", "Admin");
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        //
        // GET: /Account/ChangePassword
        [Authorize(Roles = "admin")]
        public ActionResult ChangePassword()
        {
            return this.View();
        }

        //


        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (this.ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(this.User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return this.RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    this.ModelState.AddModelError("", "Неверен текущий пароль или новый пароль с ошибками.");
                }
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess
        [Authorize(Roles = "admin")]
        public ActionResult ChangePasswordSuccess()
        {
            return this.View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult ConnectUserWithEmployee()
        {
            var model = new UserViewModel();
            model.Users = this.userRepository.GetAll().ToList();
            model.Employees = this.employeeRepository.GetAll().ToList();
            model.CurrentUser = model.Users.First();
            model.CurrentEmployee = model.Employees.First();
            return this.View(model);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult RegisterNewEmployee()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult RegisterNewEmployee(Employee employee)
        {
            if (this.ModelState.IsValid)
            {
                this.employeeRepository.Add(employee);
                return this.RedirectToAction("Index");
            }

            return this.View(employee);
        }

        [Authorize(Roles = "admin")]
        public ActionResult EmployeesList()
        {
            return this.View(this.employeeRepository.GetAll());
        }

        [Authorize(Roles = "admin")]
        public ActionResult DeleteEmployee(int id)
        {
            Employee employee = this.employeeRepository.GetById(id);
            return this.View(employee);
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
            this.employeeRepository.Delete(id);
            return this.RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public ActionResult EditEmployee(int id)
        {
            Employee employee = this.employeeRepository.GetById(id);
            return this.View(employee);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult EditEmployee(Employee employee)
        {
            if (this.ModelState.IsValid)
            {
                this.employeeRepository.Update(employee);
                return this.RedirectToAction("Index");
            }
            return this.View(employee);
        }

        [Authorize(Roles = "admin")]
        public ActionResult DetailsEmployee(int id)
        {
            Employee employee = this.employeeRepository.GetById(id);
            return this.View(employee);
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
        public ActionResult SendUserInfo(int id)
        {
            return this.Json(userRepository.GetById(id));
        }

        [HttpPost]
        public ActionResult SendEmployeeInfo(int employeeId)
        {
            var employeeToBeReturned = employeeRepository.GetById(employeeId);
            return this.Json(new
                {
                    Id = employeeToBeReturned.Id,
                    Name = employeeToBeReturned.Name
                });
        }
    }
}