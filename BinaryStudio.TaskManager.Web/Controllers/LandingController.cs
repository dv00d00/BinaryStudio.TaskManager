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
            var model = new LandingIndexModel();
            var user = userRepository.GetByName(User.Identity.Name);
            model.UserProjects    = projectRepository.GetAllProjectsForUser(user.Id);
            model.CreatorProjects = projectRepository.GetAllProjectsForTheirCreator(user.Id);
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
                   // ProjectUsers = new Collection<User>{user}
                };
            this.projectRepository.Add(project);
            var projectList = projectRepository.GetAllProjectsForUser(user.Id);
            var projectsToModel = projectList.Select(proj => new ProjectView { Id = proj.Id, Name = proj.Name }).ToList();
            var model = new LandingProjectsModel { UserProjects = projectsToModel };
            projectList = projectRepository.GetAllProjectsForTheirCreator(user.Id);
            projectsToModel = projectList.Select(proj => new ProjectView { Id = proj.Id, Name = proj.Name }).ToList();
            model.CreatorProjects = projectsToModel;
            return Json(model);
        }

        [HttpPost]
        public ActionResult DeleteProject(int projectId)
        {
            this.projectRepository.Delete(projectId);
            var user = userRepository.GetByName(User.Identity.Name);
            var projectList = projectRepository.GetAllProjectsForUser(user.Id);
            var projectsToModel = projectList.Select(proj => new ProjectView { Id = proj.Id, Name = proj.Name }).ToList();
            var model = new LandingProjectsModel { UserProjects = projectsToModel };
            projectList = projectRepository.GetAllProjectsForTheirCreator(user.Id);
            projectsToModel = projectList.Select(proj => new ProjectView { Id = proj.Id, Name = proj.Name }).ToList();
            model.CreatorProjects = projectsToModel;
            return Json(model);
        }

        [HttpPost]
        public ActionResult GetTasks(int projectId)
        {
            var user = userRepository.GetByName(User.Identity.Name);
            var projectList = projectRepository.GetAllProjectsForUser(user.Id);
            var projectsToModel = projectList.Select(proj => new ProjectView { Id = proj.Id, Name = proj.Name, Tasks = proj.Tasks}).ToList();
            var model = new LandingProjectsModel { UserProjects = projectsToModel };
            return Json(model);
        }
}
}
