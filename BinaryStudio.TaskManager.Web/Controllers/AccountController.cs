namespace BinaryStudio.TaskManager.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Security;

    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;
    using BinaryStudio.TaskManager.Web.Models;

    /// <summary>
    /// The account controller.
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// The user processor.
        /// </summary>
        private readonly IUserProcessor userProcessor;

        /// <summary>
        /// The project processor.
        /// </summary>
        private readonly IProjectProcessor projectProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userProcessor">
        /// The user processor.
        /// </param>
        /// <param name="projectProcessor">
        /// The project Processor.
        /// </param>
        public AccountController(IUserProcessor userProcessor, IProjectProcessor projectProcessor)
        {
            this.userProcessor = userProcessor;
            this.projectProcessor = projectProcessor;
        }

        /// <summary>
        /// The register new user.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult RegisterNewUser()
        {
            return this.View(new RegisterUserModel());
        }

        /// <summary>
        /// The register new user.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        public ActionResult RegisterNewUser(RegisterUserModel model)
        {
            if (ModelState.IsValid)
            {
                if (this.userProcessor.CreateUser(model.UserName, model.Password, model.Email, string.Empty))
                {
                    this.userProcessor.LogOnUser(model.UserName, model.Password);
                    var user = userProcessor.GetCurrentLoginedUser(model.UserName);
                    this.projectProcessor.CreateDefaultProject(user);
                    return this.RedirectToAction("AllManagersWithTasks", "HumanTasks");
                }
            }

            ModelState.AddModelError(string.Empty, "Wrong registration data. Please, try again!");
            return this.View(model);
        }

        /// <summary>
        /// The log on.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult LogOn()
        {
            return this.View(new LogOnModel());
        }

        /// <summary>
        /// The log on.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (this.userProcessor.LogOnUser(model.UserName, model.Password))
                {
                    this.userProcessor.SetRoleToUserFromDB(model.UserName);
                    FormsAuthentication.SetAuthCookie(model.UserName, true);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return this.Redirect(returnUrl);
                    }
                    else
                    {
                        return this.RedirectToAction("AllManagersWithTasks", "HumanTasks");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Incorrect email or password!");
                }
            }

            return this.View(model);
        }

        /// <summary>
        /// The log off.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return this.RedirectToAction("LogOn", "Account");
        }
    }

    public interface IProjectProcessor
    {
        void CreateDefaultProject(User user);
    }

    public class ProjectProcessor : IProjectProcessor
    {
        /// <summary>
        /// The project repository.
        /// </summary>
        private readonly IProjectRepository projectRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectProcessor"/> class.
        /// </summary>
        /// <param name="projectRepository">
        /// The project repository.
        /// </param>
        public ProjectProcessor(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        /// <summary>
        /// The create project.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        public void CreateDefaultProject(User user)
        {
            this.CreateProject(user, "Home Project", string.Empty);
        }

        public void CreateProject(User user, string projectName, string projectDescription)
        {
            var project = new Project
            {
                CreatorId = user.Id,
                Creator = user,
                Created = DateTime.Now,
                Name = projectName,
                Description = projectDescription
            };
            this.projectRepository.Add(project);
        }
    }
}
