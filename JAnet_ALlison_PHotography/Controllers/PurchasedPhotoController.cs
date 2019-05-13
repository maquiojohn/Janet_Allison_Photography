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
    public class PurchasedPhotoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PurchasedPhoto
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            var tempEmail = System.Web.HttpContext.Current.User.Identity.GetUserName().ToString();
            var currentUser = (from user in db.Users
                               where user.Email.Equals(tempEmail)
                               select new { user.FirstName }).ToList();
            foreach (var user in currentUser)
            {
                ViewBag.ViewName = user.FirstName;
            }

            ViewBag.ZipFilesList = getZipfileListAll();
            ViewBag.ZipFilesListUser = getZipfileList(tempEmail);
            return View(await db.PurchasedPhotoes.ToListAsync());
        }

        // GET: PurchasedPhoto/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id, byte[] Picture)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchasedPhoto purchasedPhoto = await db.PurchasedPhotoes.FindAsync(id);
            if (purchasedPhoto == null)
            {
                return HttpNotFound();
            }
            ViewBag.Photo = Picture;
            return View(purchasedPhoto);
        }

        // GET: PurchasedPhoto/Create
        [Authorize(Roles = "Admin, Employee")]
        public ActionResult Create()
        {
            getCustomerList();
            return View();
        }

        // POST: PurchasedPhoto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> Create([Bind(Include = "PurchasedPic_Id,Customer_Id,Title,Picture,UploadDate,DateTaken")]
        PurchasedPhoto purchasedPhoto, HttpPostedFileBase ImageFile)
        {
            getCustomerList();
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    purchasedPhoto.Picture = new byte[ImageFile.ContentLength];
                    ImageFile.InputStream.Read(purchasedPhoto.Picture, 0, ImageFile.ContentLength);
                }
                if (purchasedPhoto.DateTaken > purchasedPhoto.UploadDate)
                {
                    ViewBag.ErrorMessage = "The date taken cannot exceed the current date.";
                    return View(purchasedPhoto);
                }
                else
                {
                    db.PurchasedPhotoes.Add(purchasedPhoto);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", "CustomerPhoto");
                }

            }
            return View(purchasedPhoto);
        }

        // GET: PurchasedPhoto/Edit/5
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> Edit(int? id)
        {
            getCustomerList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchasedPhoto purchasedPhoto = await db.PurchasedPhotoes.FindAsync(id);
            if (purchasedPhoto == null)
            {
                return HttpNotFound();
            }
            return View(purchasedPhoto);
        }

        // POST: PurchasedPhoto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> Edit([Bind(Include = "PurchasedPic_Id,Customer_Id,Title,Picture,UploadDate,DateTaken")]
        PurchasedPhoto purchasedPhoto, HttpPostedFileBase ImageFile)
        {
            getCustomerList();
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    purchasedPhoto.Picture = new byte[ImageFile.ContentLength];
                    ImageFile.InputStream.Read(purchasedPhoto.Picture, 0, ImageFile.ContentLength);
                }
                if (purchasedPhoto.DateTaken > purchasedPhoto.UploadDate)
                {
                    ViewBag.ErrorMessage = "The date taken cannot exceed the current date.";
                    return View(purchasedPhoto);
                }
                else
                {
                    db.Entry(purchasedPhoto).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", "CustomerPhoto");
                }
            }
            return View(purchasedPhoto);
        }

        // GET: PurchasedPhoto/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchasedPhoto purchasedPhoto = await db.PurchasedPhotoes.FindAsync(id);
            if (purchasedPhoto == null)
            {
                return HttpNotFound();
            }
            var photo = (from pic in db.PurchasedPhotoes
                         where pic.PurchasedPic_Id == id
                         select new { pic.Picture }).ToList();
            foreach (var item in photo)
            {
                ViewBag.Photo = item.Picture;
            }
            return View(purchasedPhoto);
        }

        // POST: PurchasedPhoto/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PurchasedPhoto purchasedPhoto = await db.PurchasedPhotoes.FindAsync(id);
            db.PurchasedPhotoes.Remove(purchasedPhoto);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "CustomerPhoto");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //get all user that has customer role
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
        // get all zipfiles
        public IEnumerable<SelectListItem> getZipfileListAll()
        {
            List<SelectListItem> ziplist = new List<SelectListItem>();
            var zipAll = (from zip in db.ZipFiles
                          select new { zip.PurchasedPic_Id, zip.Title }).ToList();
            foreach (var item in zipAll)
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
        //get the user's zipfile
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

        public FileResult DownloadFile(FormCollection fc)
        {
            int tempId = Int32.Parse(fc["ZipFile"]);
            var FileById = (from zip in db.ZipFiles
                            where zip.PurchasedPic_Id == tempId
                            select new
                            { zip.Title, zip.Picture }).ToList().FirstOrDefault();

            return File(FileById.Picture, "image/jpg", FileById.Title + ".zip");

        }
    }
}
