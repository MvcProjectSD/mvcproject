using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC;

namespace MVC.Controllers
{
    public class MainAdminController : Controller
    {
        private MVCProjectEntities db = new MVCProjectEntities();

        // GET: MainAdmin
        public ActionResult Index()
        {
            var users = from U in db.Users
                        where U.employeeType == 2
                        select  U;

            return View(users.ToList());
        }


        public ActionResult Edit(int id)
        {
            var users = db.Users.First(U => U.User_ID == id);

            return View(users);
        }
        
    }
}
