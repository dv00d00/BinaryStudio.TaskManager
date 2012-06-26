using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Web.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Web.Models;

    /// <summary>
    /// Provides access to human task entities
    /// </summary>
    public class HumanTasksController : Controller
    {
        private readonly IHumanTaskRepository humanTaskRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="HumanTasksController"/> class.
        /// </summary>
        /// <param name="humanTaskRepository">The human task repository.</param>
        public HumanTasksController(IHumanTaskRepository humanTaskRepository)
        {
            this.humanTaskRepository = humanTaskRepository;
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

        public ActionResult Create()
        {
            ViewBag.PossibleCreators = new List<Employee>();
            ViewBag.PossibleAssignees = new List<Employee>();
            return View();
        }

        //
        // POST: /HumanTasks/Create

        [HttpPost]
        public ActionResult Create(HumanTask humanTask)
        {
            if (ModelState.IsValid)
            {
                this.humanTaskRepository.Add(humanTask);
                return RedirectToAction("Index");
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

        public ActionResult AllManagersWithTasks()
        {
            var model = CreateManagersViewModel();

            return View(model);
        }


       

        private  ManagersViewModel CreateManagersViewModel()
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
                                                    new HumanTask() {Name = "Do Something",Description = "bla bla bla"},
                                                    new HumanTask() {Name = "Do Something",Description = "bla bla bla"},
                                                    new HumanTask() {Name = "Do Something",Description = "bla bla bla"},
                                                    new HumanTask() {Name = "Do Something",Description = "bla bla bla"},
                                                    new HumanTask() {Name = "Do Something",Description = "bla bla bla"},
                                                }
                                    },
                                new ManagerTasksViewModel()
                                    {
                                        Manager = new Employee() {Name = "Petya"},
                                        Tasks =
                                            new List<HumanTask>
                                                {
                                                    new HumanTask() {Name = "Do Something",Description = "bla bla bla"},
                                                    new HumanTask() {Name = "Do Something",Description = "bla bla bla"},
                                                    new HumanTask() {Name = "Do Something",Description = "bla bla bla"},
                                                    new HumanTask() {Name = "Do Something",Description = "bla bla bla"},
                                                    new HumanTask() {Name = "Do Something",Description = "bla bla bla"},
                                                }
                                    },
                                new ManagerTasksViewModel()
                                    {
                                        Manager = new Employee() {Name = "Vanya"},
                                        Tasks =
                                            new List<HumanTask>
                                                {
                                                   new HumanTask() {Name = "Do Something",Description = "bla bla bla"},
                                                    new HumanTask() {Name = "Do Something",Description = "bla bla bla"},
                                                    new HumanTask() {Name = "Do Something",Description = "bla bla bla"},
                                                    new HumanTask() {Name = "Do Something",Description = "bla bla bla"},
                                                    new HumanTask() {Name = "Do Something",Description = "bla bla bla"},
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