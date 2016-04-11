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
    public class MemberController : Controller
    {
        //
        // GET: /Member/
        MVCProjectEntities DB = new MVCProjectEntities();
        public ActionResult Index( int id)
        {

            var memberlogin = from item in DB.Members 
                              join item2 in DB.Logins
                              on  item.Member_ID equals item2.User_ID 
                                  where item2.User_ID == id
                              select item;
            ViewBag.Message = id;
            return View(memberlogin.FirstOrDefault());
            
        }
        [HttpGet]
        public ActionResult UpdateProfile(int id)
        {
            var updating = from x in DB.Members
                           where x.Member_ID == id
                           select x;
            ViewBag.Data = "id";
            return View(updating.FirstOrDefault());
        }
        [HttpPost]
        public ActionResult UpdateProfile(Member member)
        {
            if (ModelState.IsValid)
            {
                var updated = (from t in DB.Members
                              where t.Member_ID== member.Member_ID
                              select t).ToList();
                foreach (var y in updated)
                {
                    DB.Members.Remove(y);
                }
                DB.Members.Add(member);
                DB.SaveChanges();
                return RedirectToAction("Index", "Member", new { ID = member.Member_ID });
            }
            else
                return View(member);

        }
        public ActionResult MemberHome()
        {
            return View();
        }
	}
}