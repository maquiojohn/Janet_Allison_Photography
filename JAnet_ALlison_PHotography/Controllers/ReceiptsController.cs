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

namespace JAnet_ALlison_PHotography.Controllers
{
    public class ReceiptsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Receipts
        [Authorize]
        public async Task<ActionResult> Index()
        {
            //collect all the receipts
            List<Receipt> Receipts= new List<Receipt>();
            var receipts = (from rec in db.Receipts
                           select new { rec.Address, rec.Email, rec.FirstName }).ToList();

            foreach (var rec in receipts)
            {
                Receipt recp = new Receipt();
                recp.Address = rec.Address;
                recp.Email = rec.Email;

                Receipts.Add(recp);
            }

            ViewBag.Receipts = Receipts;
            //Collect if user has a receipt
            List<Receipt> ReceiptList = new List<Receipt>();
            var tempEmail = System.Web.HttpContext.Current.User.Identity.GetUserName().ToString();

            var receipt = (from rec in db.Receipts
                           where rec.Email.Equals(tempEmail)
                           select new { rec.Address, rec.Email,rec.FirstName }).ToList();

            foreach (var rec in receipt)
            {
                Receipt recp = new Receipt();
                recp.Address = rec.Address;
                recp.Email = rec.Email;

                ReceiptList.Add(recp);
            }
            ViewBag.ReceiptList = ReceiptList;
            return View(await db.Receipts.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DetailedRerport()
        {
            return View(db.Receipts.ToList());
        }
        [Authorize(Roles = "Admin")]
        public ActionResult SummaryReport()
        {

            return View(db.Receipts.ToList());
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ExceptionReport(FormCollection form)
        {
            var search = "";
            if (form["search"] != null)
            {
                search = form["search"].ToLower();
                ViewBag.search = search;
            }
            return View(db.Receipts.ToList());

        }

        //Download file
        public FileResult DownloadFile(int? id)
        {


            var FileById = (from pic in db.ProductPictures
                            where pic.Picture_Id == id
                            select new
                            { pic.Title, pic.Picture }).ToList().FirstOrDefault();

            return File(FileById.Picture, "image/jpg", FileById.Title + ".jpg");

        }

        // GET: Receipts/Details/5
        [Authorize]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receipt receipt = await db.Receipts.FindAsync(id);
            if (receipt == null)
            {
                return HttpNotFound();
            }
            return View(receipt);
        }

        // GET: Receipts/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Receipts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Receipt_Id,FirstName,LastName,Address,PhoneNumber,Email,Receipt_Date")] Receipt receipt)
        {
            if (ModelState.IsValid)
            {
                db.Receipts.Add(receipt);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(receipt);
        }

        // GET: Receipts/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receipt receipt = await db.Receipts.FindAsync(id);
            if (receipt == null)
            {
                return HttpNotFound();
            }
            return View(receipt);
        }

        // POST: Receipts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "Receipt_Id,FirstName,LastName,Address,PhoneNumber,Email,Receipt_Date")] Receipt receipt)
        {
            if (ModelState.IsValid)
            {
                db.Entry(receipt).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(receipt);
        }

        // GET: Receipts/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receipt receipt = await db.Receipts.FindAsync(id);
            if (receipt == null)
            {
                return HttpNotFound();
            }
            return View(receipt);
        }

        // POST: Receipts/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Receipt receipt = await db.Receipts.FindAsync(id);
            db.Receipts.Remove(receipt);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
