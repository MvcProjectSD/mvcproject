using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC;
using System.IO;

namespace MVC.Controllers
{
    public class MainAdminController : Controller
    {
        private MVCProjectEntities db = new MVCProjectEntities();

        // GET: MainAdmin
        public ActionResult Index()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var users = from U in db.Users
                        where U.employeeType == 2
                        select  U;

            return View(users.ToList());
        }

        [HttpGet]
        public ActionResult Edit(int id , int type)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var users = db.Users.First(U => U.User_ID == id & U.employeeType == type);

            var login = db.Logins.FirstOrDefault(L => L.User_ID == id & L.Type == type);

            MVC.Models.logg loggs = new Models.logg() { login = login, user = users , image = new Models.ImageToUpload() };

            return View(loggs);
        }

        [HttpPost]
        public ActionResult Edit(User user , Login login , Models.ImageToUpload image)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            db.Users.FirstOrDefault(U => U.User_ID == user.User_ID & U.employeeType == user.employeeType).firstname = user.firstname;
            db.Users.FirstOrDefault(U => U.User_ID == user.User_ID & U.employeeType == user.employeeType).lastname = user.lastname;
            db.Users.FirstOrDefault(U => U.User_ID == user.User_ID & U.employeeType == user.employeeType).middlename = user.middlename;
            db.Users.FirstOrDefault(U => U.User_ID == user.User_ID & U.employeeType == user.employeeType).hiredate = user.hiredate;
            db.Users.FirstOrDefault(U => U.User_ID == user.User_ID & U.employeeType == user.employeeType).salary = user.salary;
            db.Logins.FirstOrDefault(U => U.User_ID == user.User_ID & U.Type == login.Type).UserName = login.UserName;
            db.Logins.FirstOrDefault(U => U.User_ID == user.User_ID & U.Type == login.Type).Password = login.Password;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CreateAdmin()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            return View();
        }

        [HttpPost]
        public ActionResult CreateAdmin(User user , Login login , Models.ImageToUpload image)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            byte[] array = new byte [2800];

            if (image.file != null)
            {
                string pic = System.IO.Path.GetFileName(image.file.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/images/Users"), pic);
                
                image.file.SaveAs(path);

                using (MemoryStream ms = new MemoryStream())
                {
                    image.file.InputStream.CopyTo(ms);
                    array = ms.GetBuffer();
                }

            }

            User userr = new User()
            {
                 firstname = user.firstname,
                 lastname =  user.lastname,
                 middlename = user.middlename,
                 hiredate = user.hiredate,
                 birthdate = user.birthdate,
                 Image = array,
                 phoneNo = user.phoneNo,
                 salary = user.salary,
                 employeeType = 2,
                 address = user.address,
            };

            db.Users.Add(userr);

            db.SaveChanges();

            int userid = db.Users.FirstOrDefault(U => U.firstname == user.firstname & U.lastname == user.lastname & U.middlename == user.middlename).User_ID;

            Login log = new Login() 
            {
                 User_ID = userid,
                 Login_No = 0,
                 UserName = login.UserName,
                 Password = login.Password ,
                 Type = 2
            };

            
            db.Logins.Add(log);

            db.SaveChanges();

            return  RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            db.Logins.FirstOrDefault(L => L.User_ID == id).status = false;

            db.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]

        public ActionResult AllBooks ()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            return View(db.Books.ToList());
        }
        [HttpGet]

        public ActionResult AllPublishers ()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }            
            
            var result = from B in db.Books
                         select B.publisher;

                         
            return View(result.ToList().Distinct());

        }
        [HttpGet]

        public ActionResult AllAuthors()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }


            var result = from B in db.Books
                         select B.author;


            return View(result.ToList().Distinct());

        }
        [HttpGet]
        public ActionResult Search ()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            return View();
        }
        [HttpGet]
        public ActionResult availablebooks (string search)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            if (search == null || search == string.Empty)
            {
                var result = from B in db.Books
                             where B.Available == true
                             select B;

                return View(result.ToList());
            }
            else
            {
                var result = from B in db.Books
                             where B.Available == true & B.category.ToLower() == search.ToLower()
                             select B;

                return View(result.ToList());
            }
        }
        [HttpGet]
        public ActionResult borrowedBooks ()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            var result = from B in db.borrowBooks
                         select B;

            return View(result.ToList());
        }
        [HttpGet]
        public ActionResult NewArrivedBooks(string search)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            DateTime pre = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6);

            if (search == null || search == string.Empty)
            {

                var result = from B in db.Books
                             where B.arrivedDate >= pre & B.arrivedDate <= DateTime.Now
                             select B;
                return View(result.ToList());
            }
            else
            {
                var result = from B in db.Books
                             where B.arrivedDate >= pre & B.arrivedDate <= DateTime.Now & B.category.ToLower() == search.ToLower()
                             select B;
                return View(result.ToList());
            }
        }

        [HttpGet]
        public ActionResult mostborrowedbooks( string year)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            if(year == null || year == string.Empty)
            {
                var result = db.most_borrowed_books().ToList();
                return View(result);
            }
            else
            {
                var result = db.most_borrowed_books_by_year(Int32.Parse(year)).ToList();
                return View(result);

            }
        }

        [HttpGet]
        public ActionResult mostreadingbooks(string year)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            if (year == null || year == string.Empty)
            {
                var result = db.most_reading_books().ToList();
                return View(result);
               
            }
            else
            {
                var result = db.most_reading_books_by_year(Int32.Parse(year)).ToList();
                return View(result);
                
            }
        }
        [HttpGet]
        public ActionResult logout ()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            Session["username"] =null;
            Session["type"] = null;
            Session["user_id"] = null;

            return RedirectToAction("SignIn", "Login");
        }
        [HttpGet]
        public ActionResult profile()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            int id = Int32.Parse( Session["user_id"].ToString());

            var result = from A in db.Users
                         where A.User_ID ==  id
                         select A;

            return View(result.FirstOrDefault());
        }
    }
}
