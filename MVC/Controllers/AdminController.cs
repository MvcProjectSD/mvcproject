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
            User emp = AdminEntity.Users.Find(Session["user_id"]);
            return View(emp);
        }

        public ActionResult Profile(int id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            //User admin = AdminEntity.Users.Find(id);
            var admin = from m in AdminEntity.Users
                where m.User_ID == id
                select m;
            return View(admin.SingleOrDefault());
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
        public ActionResult CreateEmployee(User emp,Login login)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            //if (ModelState.IsValid)
            //{
            //    AdminEntity.Users.Add(emp);
            //    AdminEntity.SaveChanges();
            //    return RedirectToAction("Employees");
            //}
            User usr = new User()
            { 
                firstname = emp.firstname,
                middlename = emp.middlename,
               lastname = emp.lastname,
               address = emp.address,
               birthdate = emp.birthdate,
               phoneNo = emp.phoneNo,
               employeeType = 3,
                hiredate = emp.hiredate,
                salary = emp.salary
            };
            AdminEntity.Users.Add(usr);
            //AdminEntity.SaveChanges();
            int userId =AdminEntity.Users.FirstOrDefault(u => u.firstname == emp.firstname & u.middlename == emp.middlename & u.lastname == emp.lastname).User_ID;

            Login log = new Login()
            {
                User_ID = userId,
                Login_No = 0,
                Type = 3,
                UserName = login.UserName,
                Password = login.Password,
            };

            AdminEntity.Logins.Add(log);
            AdminEntity.SaveChanges();

            return RedirectToAction("Employees");
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

        public ActionResult ChangePassword(string error)
        {
            string username = Session["username"].ToString();
            AdminEntity.Logins.FirstOrDefault(U => U.UserName == username).Login_No = 1;
            return View(error);
        }

        [HttpPost]
        public ActionResult ChangePassword(string password, string confirmpassword)
        {
            string username = Session["username"].ToString();
            if (password == confirmpassword)
            {
                AdminEntity.Logins.FirstOrDefault(L => L.UserName == username).Password = password;

                AdminEntity.SaveChanges();

                return RedirectToAction("CompleteProfile", new { id = Session["user_id"], type = Session["type"] });
            }

            else
            {
                return View("ChangePassword", "", "Password Doesn't Match");
            }
        }


        [HttpGet]
        public ActionResult CompleteProfile(int id, int type)
        {
            string username = Session["username"].ToString();

            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var users = AdminEntity.Users.First(U => U.User_ID == id & U.employeeType == type);

            var login = AdminEntity.Logins.FirstOrDefault(L => L.User_ID == id & L.Type == type);

            MVC.Models.logg loggs = new Models.logg() { login = login, user = users, image = new Models.ImageToUpload() };

            AdminEntity.Logins.FirstOrDefault(U => U.UserName == username).Login_No = 0;
            return View(loggs);
        }

        [HttpPost]
        public ActionResult CompleteProfile(User user, Login login, Models.ImageToUpload image)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            AdminEntity.Users.FirstOrDefault(U => U.User_ID == user.User_ID & U.employeeType == user.employeeType).firstname = user.firstname;
            AdminEntity.Users.FirstOrDefault(U => U.User_ID == user.User_ID & U.employeeType == user.employeeType).lastname = user.lastname;
            AdminEntity.Users.FirstOrDefault(U => U.User_ID == user.User_ID & U.employeeType == user.employeeType).middlename = user.middlename;
            AdminEntity.Users.FirstOrDefault(U => U.User_ID == user.User_ID & U.employeeType == user.employeeType).birthdate = user.birthdate;
            AdminEntity.Users.FirstOrDefault(U => U.User_ID == user.User_ID & U.employeeType == user.employeeType).address = user.address;
            AdminEntity.Users.FirstOrDefault(U => U.User_ID == user.User_ID & U.employeeType == user.employeeType).phoneNo = user.phoneNo;
            AdminEntity.Logins.FirstOrDefault(U => U.User_ID == user.User_ID & U.Type == user.employeeType).Login_No = 1;
            AdminEntity.SaveChanges();
            return RedirectToAction("Index");
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
               // member.Member_ID = AdminEntity.Members.Select(m => m.Member_ID).Max() + 1;
               int max_id = AdminEntity.Members.OrderByDescending(m => m.Member_ID).FirstOrDefault().Member_ID;
                member.Member_ID = max_id + 1;
                AdminEntity.Members.Add(member);
                AdminEntity.SaveChanges();
                return RedirectToAction("Members");
            }

            return View(member);
        }

        public ActionResult logout()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            Session["username"] = null;
            Session["type"] = null;
            Session["user_id"] = null;

            return RedirectToAction("SignIn", "Login");
        }
    }
}