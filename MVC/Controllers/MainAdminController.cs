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
            var users = from U in db.Users
                        where U.employeeType == 2
                        select  U;

            return View(users.ToList());
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var users = db.Users.First(U => U.User_ID == id);

            var login = db.Logins.FirstOrDefault(L => L.User_ID == id);

            MVC.Models.logg loggs = new Models.logg() { login = login, user = users , image = new Models.ImageToUpload() };

            return View(loggs);
        }

        [HttpPost]
        public ActionResult Edit(User user , Login login , Models.ImageToUpload image)
        {
            db.Users.FirstOrDefault(U => U.User_ID == user.User_ID).firstname = user.firstname;
            db.Users.FirstOrDefault(U => U.User_ID == user.User_ID).lastname = user.lastname;
            db.Users.FirstOrDefault(U => U.User_ID == user.User_ID).middlename = user.middlename;
            db.Users.FirstOrDefault(U => U.User_ID == user.User_ID).hiredate = user.hiredate;
            db.Users.FirstOrDefault(U => U.User_ID == user.User_ID).salary = user.salary;
            db.Logins.FirstOrDefault(U => U.User_ID == user.User_ID).UserName = login.UserName;
            db.Logins.FirstOrDefault(U => U.User_ID == user.User_ID).Password = login.Password;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAdmin(User user , Login login , Models.ImageToUpload image)
        {
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

        public ActionResult Delete(int id)
        {
            db.Logins.FirstOrDefault(L => L.User_ID == id).status = false;

            db.SaveChanges();

            return RedirectToAction("Index");
        }
        
    }
}
