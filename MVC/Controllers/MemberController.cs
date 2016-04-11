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
        MVCProjectEntities DB = new MVCProjectEntities();

        public ActionResult Index(int id)
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
                               where t.Member_ID == member.Member_ID & x.User_ID == member.Member_ID
                               select new { t, x }).FirstOrDefault();
                updated.x.UserName = login.UserName;
                updated.x.Password = login.Password;
                updated.t.FullName = member.FullName;
                updated.t.Email = member.Email;
                updated.t.Address = member.Email;
                updated.t.PhoneNumber = member.PhoneNumber;
                DB.SaveChanges();

                return RedirectToAction("Index", "Member", new { ID = member.Member_ID });
            }
            else
                return View(member);

        }

        [HttpGet]
        public ActionResult NewBooks(int id)
        {
            DateTime pre = DateTime.Now.AddDays(-6);

            var result = from B in DB.Books
                         where B.arrivedDate >= pre & B.arrivedDate <= DateTime.Now
                         select B;
            ViewBag.Message = id;
            return View(result.ToList());



        }
        public ActionResult ReadingBooks(int id )
        {
            
            Session["id"] = id;
            ViewBag.Message = id;
            var books = (from book in DB.Books
                        join book1 in DB.ReadingBooks
                        on book.Book_ID equals book1.Book_ID
                        where book1.Member_ID == id & book1.Date.Value.Month == DateTime.Now.Month & book1.Date <= DateTime.Now
                        select book).ToList();
            return View(books);
            

        }

   [HttpGet]

        public ActionResult ReadingMonth(string month)
        {
            
            var id = Convert.ToInt32(Session["id"]);
            ViewBag.Message = id;
            if (string.IsNullOrWhiteSpace(month))
            {
                
                var books = (
                              from book1 in DB.Books
                              join book in DB.ReadingBooks
                             on book1.Book_ID equals book.Book_ID

                              where book.Member_ID == id
                             select book1).ToList();
                
                return View(books);

            }
            else
            {
                var books = (from book1 in DB.Books
                             join book in DB.ReadingBooks
                            on book1.Book_ID equals book.Book_ID

                             where book.Member_ID == id & book.Date.Value.Month.ToString() == month 
                             select book1).ToList();
                return View(books);
 
            }
        }
        public ActionResult ReadingYear(string year)
   {
            var id = Convert.ToInt32(Session["id"]);
            ViewBag.Message = id;
            if (string.IsNullOrWhiteSpace(year))
            {
                
                var books = (
                              from book1 in DB.Books
                              join book in DB.ReadingBooks
                             on book1.Book_ID equals book.Book_ID

                              where book.Member_ID == id
                             select book1).ToList();
                
                return View(books);

            }
            else
            {
                var books = (from book1 in DB.Books
                             join book in DB.ReadingBooks
                            on book1.Book_ID equals book.Book_ID

                             where book.Member_ID == id & book.Date.Value.Year.ToString() == year 
                             select book1).ToList();
                return View(books);
 
            }
   }
        public ActionResult BorrowedBooks(int id)
        {
            
            ViewBag.Message = id;
            Session["id2"] = id;
            var books = (from book in DB.Books
                         join book1 in DB.borrowBooks
                         on book.Book_ID equals book1.Book_ID
                         where book1.Member_ID == id & book1.Borrow_date.Month == DateTime.Now.Month & book1.Borrow_date <= DateTime.Now
                         select book).ToList();
            return View(books);
        }
        public ActionResult CurrentBorrowed()
        {
            var id = Convert.ToInt32(Session["id2"]);
            ViewBag.Message = id;
            var books = (from book in DB.Books
                         join book1 in DB.borrowBooks
                         on book.Book_ID equals book1.Book_ID
                         where book1.Member_ID == id & book1.Borrow_date <= DateTime.Now & book1.return_date >= DateTime.Now & book1.ActualReturnDate == null
                         select new MVC.Models.currentbook { book = book, borrowedbook = book1 }).ToList();
                         return View(books);
        }

        [HttpGet]
        public ActionResult BorrowedMonth(string month)
        {
            var id = Convert.ToInt32(Session["id2"]);
            ViewBag.Message = id;
            if (string.IsNullOrWhiteSpace(month))
            {

                var books = (
                              from book1 in DB.Books
                              join book in DB.borrowBooks
                             on book1.Book_ID equals book.Book_ID

                              where book.Member_ID == id
                              select book1).ToList();

                return View(books);

            }
            else
            {
                var books = (from book1 in DB.Books
                             join book in DB.borrowBooks
                            on book1.Book_ID equals book.Book_ID

                             where book.Member_ID == id & book.Borrow_date.Month.ToString() == month
                             select book1).ToList();
                return View(books);

            }

        }
        public ActionResult BorrowedYear(string year)
        {
            var id = Convert.ToInt32(Session["id2"]);
            ViewBag.Message = id;
            if (string.IsNullOrWhiteSpace(year))
            {

                var books = (
                              from book1 in DB.Books
                              join book in DB.borrowBooks
                             on book1.Book_ID equals book.Book_ID

                              where book.Member_ID == id
                              select book1).ToList();

                return View(books);

            }
            else
            {
                var books = (from book1 in DB.Books
                             join book in DB.borrowBooks
                            on book1.Book_ID equals book.Book_ID

                             where book.Member_ID == id & book.Borrow_date.Year.ToString() == year
                             select book1).ToList();
                return View(books);

            }

        }
        public ActionResult MemberHome()
        {
            return View();
        }
    }
}