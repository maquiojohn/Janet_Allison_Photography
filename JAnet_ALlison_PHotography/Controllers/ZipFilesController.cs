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
    public class ZipFilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ZipFiles
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> Index()
        {
            var tempEmail = System.Web.HttpContext.Current.User.Identity.GetUserName().ToString();
            ViewBag.ZipFilesList = getZipfileList(tempEmail);
            return View(await db.ZipFiles.ToListAsync());
        }

        // GET: ZipFiles/Details/5
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ZipFile zipFile = await db.ZipFiles.FindAsync(id);
            if (zipFile == null)
            {
                return HttpNotFound();
            }
            return View(zipFile);
        }

        // GET: ZipFiles/Create
        [Authorize(Roles = "Admin, Employee")]
        public ActionResult Create()
        {
            getCustomerList();
            return View();
        }

        // POST: ZipFiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Employee")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PurchasedPic_Id,Customer_Id,Title,Picture,UploadDate,DateTaken")]
        ZipFile zipFile, HttpPostedFileBase ImageFile)
        {
            getCustomerList();
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    zipFile.Picture = new byte[ImageFile.ContentLength];
                    ImageFile.InputStream.Read(zipFile.Picture, 0, ImageFile.ContentLength);
                }
                if (zipFile.DateTaken > zipFile.UploadDate)
                {
                    ViewBag.ErrorMessage = "The date taken cannot exceed the current date.";
                    return View(zipFile);
                }
                else
                {
                    db.ZipFiles.Add(zipFile);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", "CustomerZipFile");
                }
            }

            return View(zipFile);
        }

        // GET: ZipFiles/Edit/5
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> Edit(int? id)
        {
            getCustomerList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ZipFile zipFile = await db.ZipFiles.FindAsync(id);
            if (zipFile == null)
            {
                return HttpNotFound();
            }
            return View(zipFile);
        }

        // POST: ZipFiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Employee")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PurchasedPic_Id,Customer_Id,Title,Picture,UploadDate,DateTaken")]
        ZipFile zipFile, HttpPostedFileBase ImageFile)
        {
            getCustomerList();
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    zipFile.Picture = new byte[ImageFile.ContentLength];
                    ImageFile.InputStream.Read(zipFile.Picture, 0, ImageFile.ContentLength);
                }
                if (zipFile.DateTaken > zipFile.UploadDate)
                {
                    ViewBag.ErrorMessage = "The date taken cannot exceed the current date.";
                    return View(zipFile);
                }
                else
                {
                    db.Entry(zipFile).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", "CustomerZipFile");
                }
                
            }
            return View(zipFile);
        }

        // GET: ZipFiles/Delete/5
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ZipFile zipFile = await db.ZipFiles.FindAsync(id);
            if (zipFile == null)
            {
                return HttpNotFound();
            }
            var name = (from pic in db.ZipFiles
                        where pic.PurchasedPic_Id == id
                        join cust in db.Users
                        on pic.Customer_Id equals cust.Id
                        select new
                        {
                            cust.FirstName,
                            cust.LastName,
                        }).ToList();
            foreach (var item in name)
            {
                ViewBag.Name = item.FirstName + " " + item.LastName;
            }
            return View(zipFile);
        }

        // POST: ZipFiles/Delete/5
        [Authorize(Roles = "Admin, Employee")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ZipFile zipFile = await db.ZipFiles.FindAsync(id);
            db.ZipFiles.Remove(zipFile);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "CustomerZipFile");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public IEnumerable<SelectListItem> getCustomerList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var usersWithRoles = (from user in db.Users
                                  from userRole in user.Roles
                                  join role in db.Roles on userRole.RoleId equals
                                  role.Id
                                  select new { user.Id, user.FirstName, user.LastName, role.Name }).ToList();
            foreach (var item in usersWithRoles)
            {

                if (item.Name.Equals("Customer"))
                {
                    SelectListItem customer = new SelectListItem
                    {
                        Value = item.Id,
                        Text = item.FirstName + " " + item.LastName
                    };
                    list.Add(customer);
                }
            }


            ViewBag.Customer = list;

            return list;
        }

        public IEnumerable<SelectListItem> getZipfileList(string userEmail)
        {
            List<SelectListItem> ziplist = new List<SelectListItem>();
            var tempId = "";
            var currentUser = (from user in db.Users
                               where user.Email.Equals(userEmail)
                               select new { user.Id }).ToList();
            foreach (var user in currentUser)
            {
                tempId = user.Id;
            }
            var zipfileList = (from zip in db.ZipFiles
                               where zip.Customer_Id.Equals(tempId)
                               select new { zip.PurchasedPic_Id, zip.Title, zip.Picture }).ToList();
            foreach (var item in zipfileList)
            {


                SelectListItem customer = new SelectListItem
                {
                    Value = item.PurchasedPic_Id.ToString(),
                    Text = item.Title
                };
                ziplist.Add(customer);

            }

            return ziplist;
        }
    }
}
