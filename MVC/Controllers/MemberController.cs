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
            Session["idi"] = id;
            ViewBag.Message = id;
            if (Session["user_id"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            else
            {
                var memberlogin = from item in DB.Members
                                  join item2 in DB.Logins
                                  on item.Member_ID equals item2.User_ID
                                  where item2.User_ID == id

                                  select item;
               
                return View(memberlogin.FirstOrDefault());
            }
        }
        [HttpGet]
        public ActionResult UpdateProfile(int id)
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            else
            {
                var updating = (from t in DB.Members
                                join x in DB.Logins
                                 on t.Member_ID equals x.User_ID
                                where t.Member_ID == id
                                select new MVC.Models.memlog { member = t, login = x }).FirstOrDefault();
                ViewBag.Message = id;

                return View(updating);
            }
        }
        [HttpPost]
        public ActionResult UpdateProfile(Member member, Login login)
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            else
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
                    updated.t.Address = member.Address;
                    updated.t.PhoneNumber = member.PhoneNumber;
                    DB.SaveChanges();

                    return RedirectToAction("Index", "Member", new { ID = member.Member_ID });
                }
                else
                    return View(member);
            }
        }

        [HttpGet]
        public ActionResult NewBooks(int id)
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            else
            {
                DateTime pre = DateTime.Now.AddDays(-6);

                var result = from B in DB.Books
                             where B.arrivedDate >= pre & B.arrivedDate <= DateTime.Now
                             select B;
                ViewBag.Message = id;
                return View(result.ToList());

            }

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
                ViewBag.books = books;
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
            if (Session["user_id"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            else
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
        [HttpGet]
        public ActionResult FindBook(string name)
        {
            var id = Convert.ToInt32(Session["idi"]);
            ViewBag.Message = id;
            if (name == null || name == string.Empty)
            {
                return View("Index");
            }
            else
            {
                var books = (from book in DB.Books
                            where book.title.ToLower() == name.ToLower() & book.Available == true
                            select book).ToList();
                
                return View(books);

            }

        }
        public ActionResult search()
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            else
            {
                var id = Convert.ToInt32(Session["idi"]);
                ViewBag.Message = id;
                return View();
            }
        }
        [HttpGet]
        public ActionResult FindYear(string year)
        {
            var id = Convert.ToInt32(Session["idi"]);
            ViewBag.Message = id;
            if (year == null || year == string.Empty)
            {
                var books = (from book in DB.Books
                             select book).ToList();

                return View(books);
            }
            else
            {
                var books = (from book in DB.Books
                             where book.arrivedDate.Value.Year.ToString() == year 
                             select book).ToList();

                return View(books);

            }
        }
        [HttpGet]
       public ActionResult FindCategory(string category)
        {
            var id = Convert.ToInt32(Session["idi"]);
            ViewBag.Message = id;
            if (category == null || category == string.Empty)
            {
                var books = (from book in DB.Books
                             select book).ToList();

                return View(books);
            }
            else
            {
                var books = (from book in DB.Books
                             where book.category == category 
                             select book).ToList();

                return View(books);

            }
        }
        [HttpGet]
        public ActionResult FindAuthor(string author)
       {
           var id = Convert.ToInt32(Session["idi"]);
           ViewBag.Message = id;
           if (author == null || author == string.Empty)
           {
               var books = (from book in DB.Books
                            select book).ToList();

               return View(books);
           }
           else
           {
               var books = (from book in DB.Books
                            where book.author.ToString() == author 
                            select book).ToList();

               return View(books);

           }
       }
        [HttpGet]
        public ActionResult FindPublisher(string publisher)
       {
           var id = Convert.ToInt32(Session["idi"]);
           ViewBag.Message = id;
           if (publisher == null || publisher == string.Empty)
           {
               var books = (from book in DB.Books
                            select book).ToList();

               return View(books);
           }
           else
           {
               var books = (from book in DB.Books
                            where book.publisher == publisher
                            select book).ToList();

               return View(books);

           }
       }
        [HttpGet]
        public ActionResult AvailableBooks()
         {
             var id = Convert.ToInt32(Session["idi"]);
             ViewBag.Message = id;
      
                 var books = (from book in DB.Books
                              where book.Available == true
                              select book).ToList();

                 return View(books);

             
         }
        [HttpGet]
        public ActionResult WishList(int id)
        {
            var s = Convert.ToInt32(Session["idi"]);
           ViewBag.Message = s;
           
           WishList wish = new WishList()
           {
               Book_ID = id,
               Member_ID = s


           };

           DB.WishLists.Add(wish);
           DB.SaveChanges();
           return View("search");

    }
        public ActionResult WishLists()
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            else
            {
                var id = Convert.ToInt32(Session["idi"]);
                ViewBag.Message = id;
                var wishbooks = (from wish in DB.WishLists
                                 join wish2 in DB.Books
                                 on wish.Book_ID equals wish2.Book_ID
                                 where wish.Member_ID == id
                                 select new MVC.Models.wishbooks { wishlist = wish, book = wish2 }).ToList();
                return View(wishbooks);
            }
        }
        [HttpGet]
        public ActionResult logout()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }

            Session["username"] = null;
            ViewBag.Message = null;
            Session["idi"] = null;
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("SignIn", "Login" );
            
        }
   
    }
}