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


        public ActionResult Edit(int id)
        {
            var users = db.Users.First(U => U.User_ID == id);

            var login = db.Logins.FirstOrDefault(L => L.User_ID == id);

            MVC.Models.logg loggs = new Models.logg() { login = login, user = users };

            return View(loggs);
        }

        [HttpGet]
        public ActionResult CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAdmin(HttpPostedFileBase file , string _username , string _password  , string _address, string _phone , decimal? _salary , string _firstname, string _lastname, string _middlename , DateTime? _hiredate , DateTime? _birthdate )
        {
            byte[] array = new byte [2800];

            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/images/Users"), pic);
                // file is uploaded
                file.SaveAs(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    array = ms.GetBuffer();
                }

            }

            User user = new User()
            {
                 firstname = _firstname,
                 lastname =  _lastname,
                 middlename = _middlename,
                 hiredate = _hiredate,
                 birthdate = (DateTime)_birthdate,
                 Image = array,
                 phoneNo = _phone,
                 salary = double.Parse( _salary.ToString()),
                 employeeType = 2,
                 address = _address,
            };

            db.Users.Add(user);

            Login login = new Login() 
            {
                 User_ID = user.User_ID,
                 Login_No = 0,
                 UserName = _username,
                 Password = _password ,
                 Type = 2
            };

            
            db.Logins.Add(login);

            db.SaveChanges();

            return  RedirectToAction("Index");
        }
        
    }
}
