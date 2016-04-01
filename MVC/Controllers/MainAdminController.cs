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
        public ActionResult CreateAdmin( string _username , string _password  , string _address, string _phone , double _salary ,int ID ,  string _firstname, string _lastname, string _middlename , DateTime _hiredate , DateTime _birthdate , byte [] _image)
        {
            User user = new User()
            {
                 firstname = _firstname,
                 lastname =  _lastname,
                 middlename = _middlename,
                 User_ID = ID,
                 hiredate = _hiredate,
                 birthdate =_birthdate,
                 Image = _image,
                 phoneNo = _phone,
                 salary = _salary,
                 employeeType = 2,
                 address = _address,
            };

            Login login = new Login() 
            {
                 User_ID = ID,
                 Login_No = 0,
                 UserName = _username,
                 Password = _password ,
                 Type = 2
            };

            db.Users.Add(user);
            db.Logins.Add(login);

            db.SaveChanges();

            return  RedirectToAction("Index");
        }
        
    }
}
