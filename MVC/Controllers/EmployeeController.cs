using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult profile(int id)
        {
            MVCProjectEntities objentity = new MVCProjectEntities();


            User employee = objentity.Users.Find(id);



            return View(employee);
        }
    }
}