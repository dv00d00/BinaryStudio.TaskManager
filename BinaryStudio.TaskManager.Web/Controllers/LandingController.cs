using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BinaryStudio.TaskManager.Web.Controllers
{
    using System.Collections.ObjectModel;

    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;
    using BinaryStudio.TaskManager.Web.Models;
    [Authorize]
    public class LandingController : Controller
    {
        //
        // GET: /Landing/
        private IProjectRepository projectRepository;

        private IUserRepository userRepository;

        public LandingController(IProjectRepository projectRepository, IUserRepository userRepository)
        {
            this.projectRepository = projectRepository;
            this.userRepository = userRepository;
        }

        public ActionResult Index()
        {
            var model = new LandingModel();
            var user = userRepository.GetByName(User.Identity.Name);
            model.Projects = projectRepository.GetAllProjectsForUser(user.Id);
            return View(model);
        }

        [HttpPost]
        public ActionResult AddProject(string projectName)
        {
            var user = userRepository.GetByName(User.Identity.Name);
            var project = new Project
                {
                    Created = DateTime.Now,
                    Creator = user,
                    Name = projectName,
                    CreatorId = user.Id,
                    ProjectUsers = new Collection<User>{user}
                };
            this.projectRepository.Add(project);
            var projectList = projectRepository.GetAllProjectsForUser(user.Id);

            var model = new LandingModel
                {
                    Projects    = projectList
                };
            return Json(model);
        }

}
}
