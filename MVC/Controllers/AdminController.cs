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
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
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
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

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
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
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
        [HttpPost,ActionName("DeleteEmployee")]
        public ActionResult DeleteEmployeeConfirmed(int id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            User user = AdminEntity.Users.Find(id);
            AdminEntity.Users.Remove(user);
            AdminEntity.SaveChanges();
            return RedirectToAction("Employees");

        }
        public ActionResult Books()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            var books = from b in AdminEntity.Books
                        select b;
            return View(books.ToList());
        }

        public ActionResult EditBooks(int? id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            if (id == null)
            {
                return  new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = AdminEntity.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBooks([Bind(Include = "Book_ID,title,author,publisher,category,edition,NoOfpages,NoOfCopies,Available,shelfNo,arrivedDate")] Book book)
        {
            if (ModelState.IsValid)
            {
                AdminEntity.Entry(book).State = EntityState.Modified;
                AdminEntity.SaveChanges();
                return RedirectToAction("Books");
            }
            return View(book);
        }

        public ActionResult DeleteBook(int? id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = AdminEntity.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        [HttpPost, ActionName("DeleteBook")]
        public ActionResult BookDeleteConfirmed(int id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            Book book = AdminEntity.Books.Find(id);
            AdminEntity.Books.Remove(book);
            AdminEntity.SaveChanges();
          return RedirectToAction("Books");
        }

        public ActionResult CreateBook()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBook([Bind(Include = "Book_ID,title,author,publisher,category,edition,NoOfpages,NoOfCopies,Available,shelfNo,arrivedDate")] Book book)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            if (ModelState.IsValid)
            {
                book.Book_ID = AdminEntity.Books.Select(b=>b.Book_ID).Max() + 1;
                AdminEntity.Books.Add(book);
                AdminEntity.SaveChanges();
                return RedirectToAction("Books");
            }

            return View(book);
        }

        public ActionResult Members()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            return View(AdminEntity.Members.ToList());
        }

        public ActionResult EditMember(int? id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = AdminEntity.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMember([Bind(Include = "Member_ID,FullName,Email,Address,PhoneNumber")] Member member)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            if (ModelState.IsValid)
            {
                AdminEntity.Entry(member).State = EntityState.Modified;
                AdminEntity.SaveChanges();
                return RedirectToAction("Members");
            }
            return View(member);
        }

        public ActionResult DeleteMember(int? id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = AdminEntity.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        [HttpPost, ActionName("DeleteMember")]
        [ValidateAntiForgeryToken]
        public ActionResult MemberDeleteConfirmed(int id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            Member member = AdminEntity.Members.Find(id);
            AdminEntity.Members.Remove(member);
            AdminEntity.SaveChanges();
            return RedirectToAction("Members");
        }
        public ActionResult CreateMember()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMember([Bind(Include = "Member_ID,FullName,Email,Address,PhoneNumber")] Member member)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            if (ModelState.IsValid)
            {
                member.Member_ID = AdminEntity.Members.Select(m => m.Member_ID).Max() + 1;
                AdminEntity.Members.Add(member);
                AdminEntity.SaveChanges();
                return RedirectToAction("Members");
            }

            return View(member);
        }

    }
}