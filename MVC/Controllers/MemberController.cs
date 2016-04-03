using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class MemberController : Controller
    {
        //
        // GET: /Member/
        MVCProjectEntities DB = new MVCProjectEntities();
        public ActionResult Index()
        {

            var memberlogin = from item in DB.Members 
                              join item2 in DB.Logins
                              on  item.Member_ID equals item2.User_ID 
                                  where item2.UserName == (String)Session["username"]
                              select item;
            return View(memberlogin.ToList());
        }
        public ActionResult UpdateProfile()
        {
            return View();
        }
        public ActionResult MemberHome()
        {
            return View();
        }
	}
}