using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class EmployeeController : Controller
    {
        MVCProjectEntities EmployeeEntity = new MVCProjectEntities();
       
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult profile(int id)
        {


            User employee = EmployeeEntity.Users.Find(id);



            return View(employee);
        }

        public ActionResult EditProfile(int id)
        {


            User employee = EmployeeEntity.Users.Find(id);



            return View(employee);
        }
        [HttpPost]
        public ActionResult EditProfile(User Emp)
        {
            var EditEmployee = (from data in EmployeeEntity.Users
                                where data.User_ID == Emp.User_ID
                                select data);
            foreach (var e in EditEmployee)
            {
                e.firstname = Emp.firstname;
                e.middlename = Emp.middlename;
                e.lastname = Emp.lastname;
                e.birthdate = Emp.birthdate;
                e.address = Emp.address;
                e.phoneNo = Emp.phoneNo;

            }
            EmployeeEntity.SaveChanges();
            return RedirectToAction("profile", new { id = Emp.User_ID });
        }
        
        
        //Member
        public ActionResult Members(string name,string typeOfSearch)
        {
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "Search by", Value = "0" });
            li.Add(new SelectListItem { Text = "Name", Value = "1" });
            li.Add(new SelectListItem { Text = "Email", Value = "2" });

            ViewData["typeOfSearch"] = li;

            var _objMemberdetail = (from data in EmployeeEntity.Members select data);
            if (!string.IsNullOrEmpty(typeOfSearch))
            {
                if (typeOfSearch == "1")
                    return View(_objMemberdetail.Where(b => b.FullName.ToLower().Contains(name.ToLower())).ToList());
                else if (typeOfSearch == "2")
                    return View(_objMemberdetail.Where(b => b.Email.ToLower().Contains(name.ToLower())).ToList());
                else
                    return View(EmployeeEntity.Members.ToList());

            }
            else
            {
                return View(EmployeeEntity.Members.ToList());
                
                //return View(_objMemberdetail.ToList());
            }
        }
        public ActionResult CreateMember()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMember([Bind(Include = "Member_ID,FullName,Email,Address,PhoneNumber")] Member member)
        {
            if (ModelState.IsValid)
            {
                EmployeeEntity.Members.Add(member);
                EmployeeEntity.SaveChanges();
                return RedirectToAction("Members");
            }

            return View(member);
        }
        public ActionResult EditMember(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = EmployeeEntity.Members.Find(id);
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
            if (ModelState.IsValid)
            {
                EmployeeEntity.Entry(member).State = EntityState.Modified;
                EmployeeEntity.SaveChanges();
                return RedirectToAction("Members");
            }
            return View(member);
        }
        public ActionResult DeleteMember(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = EmployeeEntity.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }


        [HttpPost, ActionName("DeleteMember")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = EmployeeEntity.Members.Find(id);
            EmployeeEntity.Members.Remove(member);
            EmployeeEntity.SaveChanges();
            return RedirectToAction("Members");
        }

    
        //Books
        public ActionResult AllBooks(string name) 
        { 
            var _objuserdetail = (from data in EmployeeEntity.Books select data);
            if (!string.IsNullOrEmpty(name))
            {
                return View(_objuserdetail.Where(b => b.category.ToLower().Contains(name)).ToList());
            } else
            {
                return View(_objuserdetail.ToList()); 
            }
        }
        public ActionResult TodayReturnBook()
        {
            var today = EmployeeEntity.todayReturnedBook1(); 
            return View(today.ToList());
        }
        public ActionResult LateReturnBook()
        {
            var _objuserdetail = (from data in EmployeeEntity.borrowBooks
                                  where data.return_date < data.ActualReturnDate
                                  select data);

            return View(_objuserdetail.ToList()); 

        }

       //     //var _objuserdetail = (from data in EmployeeEntity.borrowBooks
       //     //                      where data.ActualReturnDate.Value == DateTime.Now.Date
                                  
       //     //                      select data);

       //     //return View(_objuserdetail.ToList()); 


       //     return View(todayReturnedBook_Result);

       //     //var today = from data in EmployeeEntity.borrowBooks
                        
       //     //            where data.return_date==DateTime.Now 
       //     //            & data.Book_ID==data.Book.Book_ID
       //     //            select data;
           
       // }
       ////var late = from data in EmployeeEntity.borrowBooks
       ////           where data.ActualReturnDate < data.return_date
       ////           select data;

    }
}