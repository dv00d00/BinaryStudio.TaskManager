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
        [Authorize]
        public ActionResult Index()
        {
            var model = new LandingIndexModel();
            var user = userRepository.GetByName(User.Identity.Name);
            model.Projects = projectRepository.GetAllProjectsForUser(user.Id);
            var project = projectRepository.GetById(9);
            project.Tasks = new List<HumanTask> { new HumanTask
                {
                    Name = "asdasd"
                },
            new HumanTask
                {
                    Name = "lkja"
                }};
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
            var projectsToModel = projectList.Select(proj => new ProjectView { Id = proj.Id, Name = proj.Name }).ToList();
            var model = new LandingProjectsModel { Projects = projectsToModel };
            return Json(model);
        }

        [HttpPost]
        public ActionResult DeleteProject(int projectId)
        {
            this.projectRepository.Delete(projectId);
            var user = userRepository.GetByName(User.Identity.Name);
            var projectList = projectRepository.GetAllProjectsForUser(user.Id);
            var projectsToModel = projectList.Select(proj => new ProjectView { Id = proj.Id, Name = proj.Name }).ToList();
            var model = new LandingProjectsModel { Projects = projectsToModel };
            return Json(model);
        }

}
}
