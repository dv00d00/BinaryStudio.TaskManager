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
    using BinaryStudio.TaskManager.Web.Extentions;
    using BinaryStudio.TaskManager.Web.Models;
    [Authorize]
    public class LandingController : Controller
    {
        //
        // GET: /Landing/
        private readonly IProjectRepository projectRepository;

        private readonly IUserRepository userRepository;

        private readonly ITaskProcessor taskProcessor;

        private readonly IStringExtensions stringExtensions;

        public LandingController(IProjectRepository projectRepository, IUserRepository userRepository, ITaskProcessor taskProcessor, IStringExtensions stringExtensions)
        {
            this.projectRepository = projectRepository;
            this.userRepository = userRepository;
            this.taskProcessor = taskProcessor;
            this.stringExtensions = stringExtensions;
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
                    CreatorId = user.Id
                };
            projectRepository.Add(project);
            var projectList = projectRepository.GetAllProjectsForUser(user.Id);
            var projectsToModel = projectList.Select(proj => new LandingProjectModel { Id = proj.Id, Name = proj.Name }).ToList();
            var model = new LandingProjectsListModel { UserProjects = projectsToModel };
            projectList = projectRepository.GetAllProjectsForTheirCreator(user.Id);
            projectsToModel = projectList.Select(proj => new LandingProjectModel { Id = proj.Id, Name = proj.Name }).ToList();
            model.CreatorProjects = projectsToModel;
            return Json(model);
        }

        [HttpPost]
        public ActionResult DeleteProject(int projectId)
        {
            var tasks = this.taskProcessor.GetAllTasksInProject(projectId);
            foreach (var humanTask in tasks)
            {
                this.taskProcessor.DeleteTask(humanTask.Id);
            }
            projectRepository.Delete(projectId);
            var user = userRepository.GetByName(User.Identity.Name);
            var projectList = projectRepository.GetAllProjectsForUser(user.Id);

            var projectsToModel = projectList.Select(proj => new LandingProjectModel { Id = proj.Id, Name = proj.Name }).ToList();
            var model = new LandingProjectsListModel { UserProjects = projectsToModel };
            projectList = projectRepository.GetAllProjectsForTheirCreator(user.Id);
            projectsToModel = projectList.Select(proj => new LandingProjectModel { Id = proj.Id, Name = proj.Name }).ToList();
            model.CreatorProjects = projectsToModel;
            return Json(model);
        }

        [HttpGet]
        public ActionResult GetTasks(int projectId, string taskGroup)
        {
            List<HumanTask> taskList = null;
            string groupName = null;
            Project currentProject = null;
            List<LandingUserModel> usersToModel = null;
            var user = userRepository.GetByName(User.Identity.Name);
            
            if (projectId != -1)
            {
                currentProject = projectRepository.GetById(projectId);
                taskList = currentProject.Tasks.ToList();
                var usersList = projectRepository.GetAllUsersInProject(projectId).ToList();
                usersList.Add(projectRepository.GetCreatorForProject(projectId));
                usersToModel = usersList.Select(currentUser => new LandingUserModel()
                {
                    Id = currentUser.Id,
                    Name = currentUser.UserName,
                    Photo = userRepository.GetById(currentUser.Id).ImageData != null
                }).ToList();
            }
            else
            {
                switch (taskGroup)
                {
                    case "all": 
                        taskList = projectRepository.GetAllProjectsForUser(user.Id).SelectMany(proj => proj.Tasks).ToList();
                        taskList.AddRange(projectRepository.GetAllProjectsForTheirCreator(user.Id).SelectMany(proj => proj.Tasks).ToList());
                        groupName = "All Tasks";
                        break;
                    case "my":
                        taskList = projectRepository.GetAllProjectsForUser(user.Id).SelectMany(proj => proj.Tasks).Where(x => x.AssigneeId == user.Id).ToList();
                        taskList.AddRange(projectRepository.GetAllProjectsForTheirCreator(user.Id).SelectMany(proj => proj.Tasks).Where(x => x.AssigneeId == user.Id).ToList());
                        groupName = "My Tasks";
                        break;
                    case "unassigned": 
                        taskList = projectRepository.GetAllProjectsForUser(user.Id).SelectMany(proj => proj.Tasks).Where(x => x.AssigneeId == null).ToList();
                        taskList.AddRange(projectRepository.GetAllProjectsForTheirCreator(user.Id).SelectMany(proj => proj.Tasks).Where(x => x.AssigneeId == null).ToList());
                        groupName = "Unassigned";
                        break;
                }
            }
            var tasksToModel = taskList.Where(x => x.Closed == (DateTime?)null).Select(task => new LandingTaskModel
                            {
                                Id = task.Id,
                                Description = stringExtensions.Truncate(task.Description, 100),
                                Name = task.Name,
                                Priority = task.Priority,
                                Created = task.Created,
                                Creator = userRepository.GetById(task.CreatorId.GetValueOrDefault()).UserName.ToString(),
                                AssigneeId = task.AssigneeId,
                                Assignee = task.AssigneeId== null?null:userRepository.GetById(task.AssigneeId.GetValueOrDefault()).UserName.ToString(),
                                AssigneePhoto = task.AssigneeId==null?false:userRepository.GetById(task.AssigneeId.GetValueOrDefault()).ImageData!=null
                            });
            var model = new LandingProjectModel
                {
                    Id = projectId != -1 ? currentProject.Id : -1,
                    Name = projectId != -1 ? currentProject.Name : groupName,
                    Tasks = tasksToModel,
                    Users = usersToModel
                };
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
