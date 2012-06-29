﻿using System;
using System.Collections.ObjectModel;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Web.Models;

    /// <summary>
    /// Provides access to human task entities
    /// </summary>
    public class HumanTasksController : Controller
    {
        private readonly IHumanTaskRepository humanTaskRepository;

        private readonly ITaskProcessor taskProcessor;

        private readonly IEmployeeRepository employeeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="HumanTasksController"/> class.
        /// </summary>
        /// <param name="humanTaskRepository">The human task repository.</param>
        /// <param name="taskProcessor">The task processor.</param>
        public HumanTasksController(IHumanTaskRepository humanTaskRepository, ITaskProcessor taskProcessor, IEmployeeRepository employeeRepository)
        {
            this.humanTaskRepository = humanTaskRepository;
            this.taskProcessor = taskProcessor;
            this.employeeRepository = employeeRepository;
        }

        //
        // GET: /HumanTasks/

        public ViewResult Index()
        {
            var humanTasks = this.humanTaskRepository.GetAll();

            return View(humanTasks);
        }

        //
        // GET: /HumanTasks/Details/5

        public ViewResult Details(int id)
        {
            HumanTask humantask = this.humanTaskRepository.GetById(id);
            return View(humantask);
        }

        //
        // GET: /HumanTasks/Create

        public ActionResult Create(int managerId)
        {
            
            return View(new HumanTask()
                            {
                                AssigneeId = managerId,
                                CreatorId = managerId,
                                Created =  DateTime.Now,
                       
                            });
        }

        //
        // POST: /HumanTasks/Create

        [HttpPost]
        public ActionResult Create(HumanTask humanTask)
        {
            humanTask.Assigned = DateTime.Now;
            if (ModelState.IsValid)
            {
                this.taskProcessor.CreateTask(humanTask);
                return RedirectToAction("AllManagersWithTasks");
            }

            ViewBag.PossibleCreators = new List<Employee>();
            ViewBag.PossibleAssignees = new List<Employee>();
            return View(humanTask);
        }

        //
        // GET: /HumanTasks/Edit/5

        public ActionResult Edit(int id)
        {
            HumanTask humantask = this.humanTaskRepository.GetById(id);
            ViewBag.PossibleCreators = new List<Employee>();
            ViewBag.PossibleAssignees = new List<Employee>();
            return View(humantask);
        }

        //
        // POST: /HumanTasks/Edit/5

        [HttpPost]
        public ActionResult Edit(HumanTask humanTask)
        {
            if (ModelState.IsValid)
            {
                this.humanTaskRepository.Update(humanTask);
                return RedirectToAction("Index");
            }

            ViewBag.PossibleCreators = new List<Employee>();
            ViewBag.PossibleAssignees = new List<Employee>();
            return View(humanTask);
        }

        //
        // GET: /HumanTasks/Delete/5

        public ActionResult Delete(int id)
        {
            HumanTask humantask = this.humanTaskRepository.GetById(id);
            return View(humantask);
        }

        //
        // POST: /HumanTasks/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            this.humanTaskRepository.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult ManagerDetails(int id)
        {
            var model = new ManagerTasksViewModel();
            model.Manager = employeeRepository.GetById(id);
            model.Tasks = taskProcessor.GetTasksList(id).ToList();
            return View(model);
        }

        public ActionResult AllManagersWithTasks()
        {
            ManagersViewModel model = new ManagersViewModel();
            model.ManagerTasks = new List<ManagerTasksViewModel>();
            model.UnAssignedTasks = new List<HumanTask>();
            IList<Employee> employees = employeeRepository.GetAll();
            foreach (Employee employee in employees)
            {
                ManagerTasksViewModel manager = new ManagerTasksViewModel();
                manager.Manager = new Employee();
                manager.Manager = employee;
                manager.Tasks = new List<HumanTask>(); 
                manager.Tasks = humanTaskRepository.GetAllTasksForEmployee(employee.Id).ToList();
                model.ManagerTasks.Add(manager);
            }
            return View(model);
        }

        [HttpPost]
        public void MoveTask(int oldEmployeeId, int newEmployeeId, int taskId)
        {
            this.taskProcessor.MoveTask(oldEmployeeId, taskId);
        }

        private ManagersViewModel CreateManagersViewModel()
        {
            var model =new ManagersViewModel(){
                ManagerTasks = new List<ManagerTasksViewModel>
                            {
                                new ManagerTasksViewModel()
                                    {
                                        Manager = new Employee() {Name = "Vasya"},
                                        Tasks =
                                            new List<HumanTask>
                                                {
                                                    new HumanTask {Name = "Do Something",Description = "bla bla bla"},
                                                    new HumanTask {Name = "Do Something",Description = "bla bla bla"},
                                                    new HumanTask {Name = "Do Something",Description = "bla bla bla"},
                                                    new HumanTask {Name = "Do Something",Description = "bla bla bla"},
                                                    new HumanTask {Name = "Do Something",Description = "bla bla bla"},
                                                }
                                    },
                                new ManagerTasksViewModel()
                                    {
                                        Manager = new Employee() {Name = "Petya"},
                                        Tasks =
                                            new List<HumanTask>
                                                {
                                                    new HumanTask {Name = "Do Something",Description = "bla bla bla"},
                                                    new HumanTask {Name = "Do Something",Description = "bla bla bla"},
                                                    new HumanTask {Name = "Do Something",Description = "bla bla bla"},
                                                    new HumanTask {Name = "Do Something",Description = "bla bla bla"},
                                                    new HumanTask {Name = "Do Something",Description = "bla bla bla"},
                                                }
                                    },
                                new ManagerTasksViewModel()
                                    {
                                        Manager = new Employee() {Name = "Vanya"},
                                        Tasks =
                                            new List<HumanTask>
                                                {
                                                    new HumanTask {Name = "Do Something",Description = "bla bla bla"},
                                                    new HumanTask {Name = "Do Something",Description = "bla bla bla"},
                                                    new HumanTask {Name = "Do Something",Description = "bla bla bla"},
                                                    new HumanTask {Name = "Do Something",Description = "bla bla bla"},
                                                    new HumanTask {Name = "Do Something",Description = "bla bla bla"},
                                                }
                                    },
                            },
                            UnAssignedTasks = new List<HumanTask>()
                            { 
                                new HumanTask() {Name = "Pick Some One",Description = "do do do do"},
                                new HumanTask() {Name = "Pick Some One",Description = "do do do do"},
                                new HumanTask() {Name = "Pick Some One",Description = "do do do do"},
                            }
                        };
            
            return model;
        }
    }
}