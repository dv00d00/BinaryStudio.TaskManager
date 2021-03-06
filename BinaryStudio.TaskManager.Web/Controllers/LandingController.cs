﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BinaryStudio.TaskManager.Web.Controllers
{
    using System.Collections.ObjectModel;

    using BinaryStudio.TaskManager.Extensions;
    using BinaryStudio.TaskManager.Extensions.Extentions;
    using BinaryStudio.TaskManager.Extensions.Models;
    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;
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

        private readonly IProjectProcessor projectProcessor;

        private readonly INotifier notifier;

        private readonly INewsProcessor newsProcessor;

        public LandingController(IProjectRepository projectRepository, IUserRepository userRepository, 
            ITaskProcessor taskProcessor, IStringExtensions stringExtensions, IProjectProcessor projectProcessor, 
            INotifier notifier, INewsProcessor newsProcessor)
        {
            this.projectRepository = projectRepository;
            this.userRepository = userRepository;
            this.taskProcessor = taskProcessor;
            this.stringExtensions = stringExtensions;
            this.projectProcessor = projectProcessor;
            this.notifier = notifier;
            this.newsProcessor = newsProcessor;
        }

        public ActionResult Index()
        {
            var user = userRepository.GetByName(User.Identity.Name);
            var projectList = projectRepository.GetAllProjectsForUser(user.Id);
            var projectsToModel = projectList.Select(proj => new LandingProjectModel
                {
                    Id = proj.Id, 
                    Name = proj.Name, 
                    Creator = proj.Creator.UserName
                }).ToList();
            var model = new LandingProjectListModel { UserProjects = projectsToModel };
            projectList = projectRepository.GetAllProjectsForTheirCreator(user.Id);
            projectsToModel = projectList.Select(proj => new LandingProjectModel
                {
                    Id = proj.Id,
                    Name = proj.Name, 
                    Creator = proj.Creator.UserName
                }).ToList();
            model.CreatorProjects = projectsToModel;
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
            var id = projectRepository.Add(project);
            var projectsToModel = new LandingProjectModel
                { 
                    Id = id,
                    Name = projectName 
                };
            return Json(projectsToModel);
        }

        [HttpPost]
        public void DeleteProject(int projectId)
        {
            var tasks = this.taskProcessor.GetAllTasksInProject(projectId);
            foreach (var humanTask in tasks)
            {
                this.taskProcessor.DeleteTask(humanTask.Id);
            }
            projectRepository.Delete(projectId);
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
                var usersList = projectProcessor.GetUsersAndCreatorInProject(projectId).ToList();
                usersToModel = usersList.OrderBy(x => x.UserName).Select(currentUser => new LandingUserModel()
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
                                Description = stringExtensions.Truncate(task.Description, 70),
                                Name = stringExtensions.Truncate(task.Name, 70),
                                Priority = task.Priority,
                                Created = task.Created,
                                Creator = userRepository.GetById(task.CreatorId.GetValueOrDefault()).UserName.ToString(),
                                AssigneeId = task.AssigneeId,
                                Assignee = task.AssigneeId== null?null:userRepository.GetById(task.AssigneeId.GetValueOrDefault()).UserName.ToString(),
                                AssigneePhoto = task.AssigneeId==null?false:userRepository.GetById(task.AssigneeId.GetValueOrDefault()).ImageData!=null,
                                Finished = task.Finished
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

        [HttpPost]
        public void CreateTask(LandingCreateTaskModel model)
        {
            var creatorId = this.userRepository.GetByName(User.Identity.Name).Id;

            var task = new HumanTask
            {
                Created = DateTime.Now,
                CreatorId = creatorId,
                Description = "",
                Name = model.Name,
                Priority = model.Priority,
                ProjectId = model.ProjectId,
                AssigneeId = model.AssigneeId,
                Finished = model.FinishDate,
                Assigned = model.AssigneeId != null ? DateTime.Now : (DateTime?)null 
            };
            this.taskProcessor.CreateTask(task);

            var taskHistory = new HumanTaskHistory
            {
                NewDescription = task.Description,
                ChangeDateTime = DateTime.Now,
                NewAssigneeId = task.AssigneeId,
                NewName = task.Name,
                Task = task,
                NewPriority = task.Priority,
                Action = ChangeHistoryTypes.Create,
                UserId = this.userRepository.GetByName(User.Identity.Name).Id
            };
            this.taskProcessor.AddHistory(taskHistory);
            this.notifier.CreateTask(task.Id);
            this.newsProcessor.CreateNewsForUsersInProject(taskHistory, task.ProjectId);
        }
    }
}
