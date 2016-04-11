using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class AdminController : Controller
    {
        MVCProjectEntities AdminEntity = new MVCProjectEntities();
        //
        // GET: /Admin/
        public ActionResult Index()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            return View();
        }

        public ActionResult Profile(int id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            User admin = AdminEntity.Users.Find(id);
            return View(admin);
        }

        public ActionResult EditProfile(int id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            User admin = AdminEntity.Users.Find(id);
            return View(admin);
        }

        [HttpPost]
        public ActionResult EditProfile(User admin)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            var EditAdmin = from user in AdminEntity.Users
                            where user.User_ID == admin.User_ID
                            select user;
            foreach (var a in EditAdmin)
            {
                a.firstname = admin.firstname;
                a.middlename = admin.middlename;
                a.lastname = admin.lastname;
                a.birthdate = admin.birthdate;
                a.address = admin.address;
                a.phoneNo = admin.phoneNo;
            }

            AdminEntity.SaveChanges();
            return RedirectToAction("Profile", new { id = admin.User_ID });

        }

        public ActionResult Employees(string name, string typeOfSearch)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem { Text = "Search By", Value = "0" });
            lst.Add(new SelectListItem { Text = "FirstName", Value = "1" });
            lst.Add(new SelectListItem { Text = "LastName", Value = "2" });
            ViewData["typeOfSearch"] = lst;

            var emp = from e in AdminEntity.Users
                      where e.employeeType == 3
                      select e;

            if (!string.IsNullOrEmpty(typeOfSearch))
            {
                if (typeOfSearch == "1")
                {
                    return View(emp.Where(em => em.firstname.ToLower().Contains(name.ToLower())).ToList());
                }
                else if (typeOfSearch == "2")
                {
                    return View(emp.Where(em => em.lastname.ToLower().Contains(name.ToLower())).ToList());
                }
                else
                {
                    return View(emp);
                }
            }
            else
            {
                return View(emp);
            }

        }
        [HttpGet]
        public ActionResult CreateEmployee()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult CreateEmployee(User emp)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            if (ModelState.IsValid)
            {
                AdminEntity.Users.Add(emp);
                AdminEntity.SaveChanges();
                return RedirectToAction("Employees");
            }
            return View(emp);
        }

        public ActionResult EditEmployee(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = AdminEntity.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult EditEmployee(User user)
        {
            if (ModelState.IsValid)
            {
                AdminEntity.Entry(user).State = EntityState.Modified;
                AdminEntity.SaveChanges();
                return RedirectToAction("Employees");
            }
            return View(user);
        }

        public ActionResult DeleteEmployee(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = AdminEntity.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);

        }

        public ActionResult Books()
        {
            var books = from b in AdminEntity.Books
                        select b;
            return View(books.ToList());
        }
    }
}