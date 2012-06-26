using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BinaryStudio.TaskManager.Web.Models;



namespace BinaryAcademia.AllManagerView.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
         
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
      
       
    }
}
