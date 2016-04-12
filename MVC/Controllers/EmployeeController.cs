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
        static int BookID;
        [HttpGet]
        public ActionResult Index()
        {
            User employee = EmployeeEntity.Users.Find(Session["user_id"]);
            return View(employee);
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
        public ActionResult Members(string name, string typeOfSearch)
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


        // Books
        public ActionResult AllBooks(string name, string typeOfSearch)
        {
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "Search by", Value = "0" });
            li.Add(new SelectListItem { Text = "Category", Value = "1" });
            li.Add(new SelectListItem { Text = "Publisher", Value = "2" });
            li.Add(new SelectListItem { Text = "Publishing date", Value = "3" });
            li.Add(new SelectListItem { Text = "Author", Value = "4" });


            ViewData["typeOfSearch"] = li;
            var _objuserdetail = (from data in EmployeeEntity.Books select data);
            if (!string.IsNullOrEmpty(name))
            {
                if (typeOfSearch == "1")


                    return View(_objuserdetail.Where(b => b.category.ToLower().Contains(name)).ToList());
                else if (typeOfSearch == "2")
                    return View(_objuserdetail.Where(b => b.publisher.ToLower().Contains(name)).ToList());
                else if (typeOfSearch == "3")
                    return View(_objuserdetail.Where(b => b.Publishing_date == DateTime.Parse(name)).ToList());
                else if (typeOfSearch == "4")
                    return View(_objuserdetail.Where(b => b.author.ToLower().Contains(name)).ToList());

                else
                    return View(_objuserdetail.ToList());
            }
            else
            {
                return View(_objuserdetail.ToList());
            }
        }



        public ActionResult TodayReturnBook()
        {
            var today = EmployeeEntity.todayReturnedBook();
            return View(today.ToList());
        }
        public ActionResult LateReturnBook()
        {
            var _objuserdetail = (from data in EmployeeEntity.borrowBooks
                                  where data.return_date < data.ActualReturnDate
                                  select data);

            return View(_objuserdetail.ToList());

        }
        [HttpGet]
        public ActionResult ReadingBooks()
        {
            return View();

        }
        [HttpPost]
        public ActionResult ReadingBooks(ReadingBook read)
        {
            EmployeeEntity.ReadingBooks.Add(read);
            EmployeeEntity.SaveChanges();

            return RedirectToAction("Details");

        }
        [HttpGet]
        public ActionResult Details()
        {

            var _objuserdetail = (from data in EmployeeEntity.ReadingBooks select data);
            return View(_objuserdetail.ToList());
        }

        public ActionResult DeleteReadingBook(int Id)
        {

            EmployeeEntity.ReadingBooks.Remove(EmployeeEntity.ReadingBooks.Find(Id));
            EmployeeEntity.SaveChanges();
            return RedirectToAction("Details");

        }
        [HttpGet]
        public ActionResult EditReadingBook(int Id)
        {

            ReadingBook read = EmployeeEntity.ReadingBooks.Find(Id);
            return View(read);

        }
        [HttpPost]
        public ActionResult EditReadingBook(ReadingBook read)
        {

            EmployeeEntity.ReadingBooks.Remove(EmployeeEntity.ReadingBooks.Find(read.Reading_ID));

            EmployeeEntity.ReadingBooks.Add(read);
            EmployeeEntity.SaveChanges();
            return RedirectToAction("Details");

        }

        [HttpGet]
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

                var result = from B in EmployeeEntity.Books
                             where B.arrivedDate >= pre & B.arrivedDate <= DateTime.Now
                             select B;
                return View(result.ToList());
            }
            else
            {
                var result = from B in EmployeeEntity.Books
                             where B.arrivedDate >= pre & B.arrivedDate <= DateTime.Now & B.category.ToLower() == search.ToLower()
                             select B;
                return View(result.ToList());
            }
        }
        public ActionResult borrowThisBook(int id)
        {

            BookID = id;
           
            var borow = (from b in EmployeeEntity.borrowBooks
                         where b.Book_ID == id
                         select b).ToList();
            var book= (from b in EmployeeEntity.Books
                       where b.Book_ID == id
                       select b).ToList();


            foreach (var d in book)
            {
                if (d.NoOfCopies - borow.Count > 1)
                {
                    return View();
                }
                else
                {
                    ViewBag.Message = 0;
                     

                }

            }
                return RedirectToAction("AllBooks");


        }
        [HttpPost]
        public ActionResult borrowThisBook(borrowBook br)
        {
            br.Book_ID = BookID;
            int max_id = (EmployeeEntity.borrowBooks.OrderByDescending(u => u.Borrow_ID).FirstOrDefault().Borrow_ID);
            br.Borrow_ID = max_id + 1;
            var bdate = br.Borrow_date;
            var rdate=  br.return_date;


            var memb = (from data in EmployeeEntity.borrowBooks
                       where data.Member_ID == br.Member_ID & data.ActualReturnDate == null &data.Book_ID==BookID
                       select data).ToList();

            if ((rdate - bdate).Days <= 20)
            {
                if (memb.Count == 0)
                {
                    EmployeeEntity.borrowBooks.Add(br);
                    EmployeeEntity.SaveChanges();
                }
            }
            else
            {
               // string myString = "duration musn't greater than 20 day";
              //  return View((object)myString);
                return RedirectToAction("borrowThisBook");
            }
            return RedirectToAction("AllBooks");

        }

    }
}
