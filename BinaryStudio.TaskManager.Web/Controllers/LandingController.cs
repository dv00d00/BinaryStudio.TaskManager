using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BinaryStudio.TaskManager.Web.Controllers
{
    using BinaryStudio.TaskManager.Logic.Domain;
    using BinaryStudio.TaskManager.Web.Models;

    public class LandingController : Controller
    {
        //
        // GET: /Landing/

        public ActionResult Index()
        {
            var model = new LandingModel();
                model.Projects = new List<Project>{
                new Project { Id = 1, Name = "1asdfsdf", Description = "1deasdasdasdas" },
                new Project { Id = 2,  Name = "2asdfsdf", Description = "2deasdasdasdas" },
                new Project { Id = 3,  Name = "3asdfsdf", Description = "3deasdasdasdas" },
                new Project { Id = 4,  Name = "4asdfsdf", Description = "4deasdasdasdas" }
            };
            return this.View(model);
        }

    }
}
