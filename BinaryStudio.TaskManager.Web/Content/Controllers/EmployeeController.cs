using System.Web.Mvc;
using BinaryStudio.TaskManager.Logic.Domain;
using BinaryStudio.TaskManager.Logic.Core;

namespace BinaryStudio.TaskManager.Web.Controllers
{   
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository employeeRepository ;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        //
        // GET: /Employee/

        public ViewResult Index()
        {
            return View(employeeRepository.GetAll());
        }

        //
        // GET: /Employee/Details/5

        public ViewResult Details(int id)
        {
            Employee employee = employeeRepository.GetById(id);
            return View(employee);
        }

        //
        // GET: /Employee/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Employee/Create

        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employeeRepository.Add(employee);
                return RedirectToAction("Index");  
            }

            return View(employee);
        }
        
        //
        // GET: /Employee/Edit/5
 
        public ActionResult Edit(int id)
        {
            Employee employee = employeeRepository.GetById(id);
            return View(employee);
        }

        //
        // POST: /Employee/Edit/5

        [HttpPost]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employeeRepository.Update(employee);
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        //
        // GET: /Employee/Delete/5
 
        public ActionResult Delete(int id)
        {
            Employee employee = employeeRepository.GetById(id);
            return View(employee);
        }

        //
        // POST: /Employee/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            employeeRepository.Delete(id);
            return RedirectToAction("Index");
        }

        
    }
}