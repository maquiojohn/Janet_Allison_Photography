using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JAnet_ALlison_PHotography.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace JAnet_ALlison_PHotography.Controllers
{
    public class BookingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> Index()
        {


            return View(await db.Bookings.ToListAsync());
        }

        // GET: Booking/Details/5
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = await db.Bookings.FindAsync(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Booking/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.photoType = getPhotoType();

            ViewBag.Employee = getEmployeeList();

            return View();
        }

        [Authorize]
        public ActionResult Success()
        {
            return View();
        }

        // POST: Booking/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "booking_Id,employee_Id,UserName,Name,dateTime,PhotoType,Medium,NumPeople,PhotoDetail")] Booking booking)
        {
            ViewBag.photoType = getPhotoType();

            ViewBag.Employee = getEmployeeList();


            if (ModelState.IsValid)
            {
                var tempUserId = "";
                // gets the username of the current user
                booking.UserName = System.Web.HttpContext.Current.User.Identity.GetUserName();
                //get the infor of the current user using its email
                var userInfo = (from user in db.Users
                                where user.Email.Equals(booking.UserName)
                                select new
                                { user.FirstName, user.LastName, user.Id }).ToList();
                //setting name equals to firstname and lastname
                foreach (var user in userInfo)
                {
                    booking.Name = user.FirstName + " " + user.LastName;
                    tempUserId = user.Id;
                }


                db.Bookings.Add(booking);
                await UserManager.SendEmailAsync(tempUserId, "Booking Confirmation with Janet Allison Photography", "Thank You for booking a session with Janet Allison Photography, you will be contacted shortly by phone to confirm the appointment.<br/><br/>" +
                    "Booking Detail: <br/> Date: " + booking.dateTime.ToString("dd/MM/yyyy") + "<br/> Photo Type: " + booking.PhotoType + "<br/> Photo Detail: " +
                                                    booking.PhotoDetail + "<br/> Number of People: " + booking.NumPeople + "<br/> Medium: " + booking.Medium + "<br/><br/> <b>Janet Allison Photography</b><br /><b>Phone Number:</b> 518-645-6858<br /><b>Email:</b> Janet.l.a.w@outlook.com");
                await db.SaveChangesAsync();
                ViewBag.BookSuccess = "Booking Success";
                return RedirectToAction("Success");

            }
            return View(booking);
        }

        // GET: Booking/Edit/5
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> Edit(int? id)
        {
            ViewBag.photoType = getPhotoType();
            ViewBag.Employee = getEmployeeList();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = await db.Bookings.FindAsync(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Booking/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "booking_Id,employee_Id,UserName,Name,dateTime,PhotoType,Medium,NumPeople,PhotoDetail")] Booking booking)
        {

            ViewBag.photoType = getPhotoType();

            ViewBag.Employee = getEmployeeList();

            if (ModelState.IsValid)
            {

                // gets the username of the current user
                booking.UserName = System.Web.HttpContext.Current.User.Identity.GetUserName();
                //get the infor of the current user using its email
                var userInfo = (from user in db.Users
                                where user.Email.Equals(booking.UserName)
                                select new
                                { user.FirstName, user.LastName }).ToList();
                //setting name equals to firstname and lastname
                foreach (var user in userInfo)
                {
                    booking.Name = user.FirstName + " " + user.LastName;
                }

                db.Entry(booking).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "BookingDetail");
            }
            return View(booking);
        }

        // GET: Booking/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = await db.Bookings.FindAsync(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Booking/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Booking booking = await db.Bookings.FindAsync(id);
            db.Bookings.Remove(booking);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "BookingDetail");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public IEnumerable<SelectListItem> getEmployeeList()
        {
            List<SelectListItem> list = new List<SelectListItem>(); // greate a list to populate with employee user

            var usersWithRoles = (from user in db.Users
                                  from userRole in user.Roles
                                  join role in db.Roles on userRole.RoleId equals
                                  role.Id
                                  select new { user.Id, user.FirstName, user.LastName, role.Name }).ToList();
            foreach (var item in usersWithRoles)
            {
                if (item.Name.Equals("Employee"))
                {
                    list.Add(new SelectListItem() { Value = item.Id, Text = item.FirstName + " " + item.LastName });
                }
            }

            return list;
        }

        public IEnumerable<SelectListItem> getPhotoType()
        {
            List<SelectListItem> photoType = new List<SelectListItem>(); // list for photo type

            photoType.Add(new SelectListItem() { Value = "Pet", Text = "Pet" });
            photoType.Add(new SelectListItem() { Value = "Portrait", Text = "Portrait" });
            photoType.Add(new SelectListItem() { Value = "Event", Text = "Event" });
            photoType.Add(new SelectListItem() { Value = "Wedding", Text = "Wedding" });

            return photoType;
        }
    }
}