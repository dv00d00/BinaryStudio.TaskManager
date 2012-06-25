using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BinaryAcademia.AllManagerView.Models;
using BinaryStudio.TaskManager.Web.Content;


namespace BinaryAcademia.AllManagerView.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            AllManagersDataTest allManagersDataTest = new AllManagersDataTest();
            ViewBag.Managers = allManagersDataTest.GetManagers();
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AllManagersWithTasks()
        {

            //Create some customize managers and tasks for testing AllManagerWithTasks View
            AllManagersDataTest testManagersData = new AllManagersDataTest();
            return View(testManagersData.GetManagers());
        }
        [HttpGet]
        public ActionResult ManagersTask(int id)
        {
            AllManagersDataTest allManagersDataTest = new AllManagersDataTest();
            ICollection<ManagerModel> managers = allManagersDataTest.GetManagers();
            return View(managers.Single(x => x.ManagerId == id).Tasks);
        }
    }
}
