using System;
using System.Collections.Generic;
using System.Web.Mvc;


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
