using JAnet_ALlison_PHotography.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JAnet_ALlison_PHotography.ViewModel;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Net;

namespace JAnet_ALlison_PHotography.Controllers
{
    public class BookingDetailController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BookingDetail
        [Authorize(Roles = "Admin, Employee")]
        public ActionResult Index()
        {

            return View(getBookingDetails()); //List of BookingDetail 
        }
        [Authorize(Roles = "Admin")]
        public ActionResult DetailedReport()
        {
            return View(getBookingDetails());
        }
        [Authorize(Roles = "Admin")]
        public ActionResult SummaryReport()
        {

            return View(getBookingDetails());
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ExceptionReport(FormCollection form)
        {
            /*var search = "";
            if (form["search"] != null)
            {
                search = form["search"].ToLower();
                ViewBag.search = search;
            }*/
            //else
            //{
            //    ViewBag.search = -1;
            //}
            var search = "";
            if (form["search"] != null)
            {
                search = form["search"];
                ViewBag.search = search;
            }

            return View(getBookingDetails());
        }
        public IEnumerable<BookingDetail> getBookingDetails()
        {

            List<BookingDetail> BookingDetailList = new List<BookingDetail>(); // to hold list of Customer and Booking details 
            var BookList = (from emp in db.Users
                            join book in db.Bookings
                            on emp.Id equals book.employee_Id
                            select new
                            {
                                emp.FirstName,
                                emp.LastName,
                                book.booking_Id,
                                book.UserName,
                                book.Name,
                                book.dateTime
                                  ,
                                book.PhotoType,
                                book.PhotoDetail,
                                book.Medium,
                                book.NumPeople
                            }).ToList();

            //query getting data from database from joining two tables and storing data in customerlist 

            foreach (var item in BookList)

            {

                BookingDetail obj = new BookingDetail(); // ViewModel 
                obj.booking_Id = item.booking_Id;
                obj.Emp_Name = item.FirstName + " " + item.LastName;
                obj.UserName = item.UserName;
                obj.Name = item.Name;
                obj.dateTime = item.dateTime;
                obj.PhotoType = item.PhotoType;
                obj.PhotoDetail = item.PhotoDetail;
                obj.Medium = item.Medium;
                obj.NumPeople = item.NumPeople;

                BookingDetailList.Add(obj);

            }

            return BookingDetailList;
        }

    }
}