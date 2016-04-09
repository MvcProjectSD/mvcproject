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
using MVC.Models;



namespace MVC.Controllers
{
    public class MemberController : Controller
    {
        //
        // GET: /Member/
        MVCProjectEntities DB = new MVCProjectEntities();
        
        public ActionResult Index( int id)
        {

            var memberlogin = from item in DB.Members
                              join item2 in DB.Logins
                              on item.Member_ID equals item2.User_ID
                              where item2.User_ID == id
                              select item;
            ViewBag.Message = id;
            return View(memberlogin.FirstOrDefault());
            
        }
        [HttpGet]
        public ActionResult UpdateProfile(int id)
        {
            var updating = (from t in DB.Members
                            join x in DB.Logins
                             on t.Member_ID equals x.User_ID
                            where t.Member_ID == id
                            select new MVC.Models.Memlog { member = t, login = x }).FirstOrDefault();
            ViewBag.Message = id;
            
            return View(updating);
        }
        [HttpPost]
        public ActionResult UpdateProfile(Member member, Login login)
        {
            if (ModelState.IsValid)
            {
                var updated = (from t in DB.Members
                               join x in DB.Logins
                               on t.Member_ID equals x.User_ID
                               where t.Member_ID == member.Member_ID & x.User_ID== member.Member_ID
                               select new { t, x }).FirstOrDefault();
                updated.x.UserName = login.UserName;
                updated.x.Password = login.Password;
                updated.t.FullName = member.FullName;
                updated.t.Email = member.Email;
                updated.t.Address = member.Email;
                updated.t.PhoneNumber = member.PhoneNumber;

                
                try
                {
                    DB.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                
                return RedirectToAction("Index", "Member", new { ID = member.Member_ID });
            }
            else
                return View(member);

        }

        [HttpGet]
        public ActionResult NewBooks(int id)
        {
            DateTime pre = DateTime.Now.AddDays(- 6);

                var result = from B in DB.Books
                             where B.arrivedDate >= pre & B.arrivedDate <= DateTime.Now
                             select B;
                ViewBag.Message = id;
                return View(result.ToList());
                
   

        }
        
        public ActionResult MemberHome()
        {
            return View();
        }
	}
}