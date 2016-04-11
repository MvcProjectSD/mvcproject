using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult SignIn()
        {
            Login _objuserloginmodel = new Login();
            return View(_objuserloginmodel);
        }
        [HttpPost]
        public ActionResult SignIn(Login _objuserloginmodel)
        {
            MVCProjectEntities objentity = new MVCProjectEntities();
            var _objuserdetail = (from data in objentity.Logins
                                  where data.UserName == _objuserloginmodel.UserName
                                  && data.Password == _objuserloginmodel.Password
                                  && data.status == true
                                  select data
                                  );

            if (_objuserdetail.Count() > 0)
            {
                objentity.Logins.FirstOrDefault(U => U.UserName == _objuserloginmodel.UserName).Login_No = 
                    objentity.Logins.Where(l => l.UserName == _objuserloginmodel.UserName).Select(L=> L.Login_No).FirstOrDefault() + 1;

                objentity.SaveChanges();

                foreach (var h in _objuserdetail)
                {
                    switch (h.Type)
                    {
                        case 1:
                            Session["username"] = _objuserloginmodel.UserName;
                            
                            

                            var MainAdminID = (from data in objentity.Users
                                               where data.User_ID == h.User_ID
                                                select data);
                            Session["type"] =  MainAdminID.Select(X => X.employeeType).FirstOrDefault();;
                            Session["user_id"] =  MainAdminID.Select(X => X.User_ID).FirstOrDefault();

                            foreach (var n in MainAdminID)
                            {
                                return RedirectToAction("Index", "MainAdmin");
                            }
                            break;

                        case 2:
                            Session["username"] = _objuserloginmodel.UserName;
                            Session["type"] = "Admin";

                            var AdminID = (from data in objentity.Users
                                           where data.User_ID == h.User_ID
                                           select data);

                            Session["type"] = AdminID.Select(X => X.employeeType).FirstOrDefault();
                            Session["user_id"] = AdminID.Select(X => X.User_ID).FirstOrDefault();


                            foreach (var n in AdminID)
                            {
                                return RedirectToAction("profile", "Admin", new { id = n.User_ID });
                            }
                            break;

                        case 3:
                            Session["username"] = _objuserloginmodel.UserName;
                            Session["type"] = "Employee";
                            var EmployeeID = (from data in objentity.Users
                                            where data.User_ID == h.User_ID
                                            select data);


                            Session["type"] = EmployeeID.Select(X => X.employeeType).FirstOrDefault();
                            Session["user_id"] = EmployeeID.Select(X => X.User_ID).FirstOrDefault();


                            foreach (var n in EmployeeID)
                            {
                                return RedirectToAction("profile", "Employee", new { id = n.User_ID });
                            }
                            break;

                        case 4:

                            Session["username"] = _objuserloginmodel.UserName;
                            Session["type"] = "Member";
                            var MemberID = (from data in objentity.Users
                                            where data.User_ID == h.User_ID
                                            select data);


                            Session["type"] = MemberID.Select(X => X.employeeType).FirstOrDefault();
                            Session["user_id"] = MemberID.Select(X => X.User_ID).FirstOrDefault();

                            foreach (var n in MemberID)
                            {
                                return RedirectToAction("Index", "Member", new { id = n.User_ID });
                            }
                            break;


                        default:

                            break;

                    }
                }
            }
            else
            {
                ViewBag.Message = 0;
            }

            return View();


        }
    }
}