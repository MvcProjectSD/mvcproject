using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult Members()
        {

            return View(EmployeeEntity.Members.ToList());
        }

        public ActionResult AllBooks(string name)
        {
            var _objuserdetail = (from data in EmployeeEntity.Books


                                  select data
                                  );
            if (!string.IsNullOrEmpty(name))
            {



                return View(_objuserdetail.Where(b => b.category.ToLower().Contains(name)).ToList());

            }
            else
            {

                return View(_objuserdetail.ToList());
            }
        }

        public ActionResult BooksByCatogure()
        {
            var _objuserdetail = (from data in EmployeeEntity.Books
                                  group data by data.category
                                  into g
                                  select new
                                  {


                                  }
                                   );


            return View(EmployeeEntity.Books.ToList());
        }
        public ActionResult NewArrivedBooks()
        {

            DateTime pre = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6);
            var _objuserdetail = (from data in EmployeeEntity.Books

                                  where data.arrivedDate <= DateTime.Now & data.arrivedDate >=pre
                                  select data
                                  );
            return View(_objuserdetail.ToList());

        }
    }
}