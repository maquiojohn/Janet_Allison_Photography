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

namespace JAnet_ALlison_PHotography.Controllers
{
    public class ProductPictureController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();


        // GET: ProductPicture
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> Index()
        {
            return View(await db.ProductPictures.ToListAsync());
        }

        // GET: ProductPicture/Details/5
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductPicture productPicture = await db.ProductPictures.FindAsync(id);
            if (productPicture == null)
            {
                return HttpNotFound();
            }
            return View(productPicture);
        }

        // GET: ProductPicture/Create
        [Authorize(Roles = "Admin, Employee")]
        public ActionResult Create()
        {

            ViewBag.Employee = getEmployeeList();
            return View();
        }

        // POST: ProductPicture/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> Create([Bind(Include = "Picture_Id, employee_Id, Title, Picture, Price, Description")]
        ProductPicture productPicture, HttpPostedFileBase ImageFile)
        {
            ViewBag.Employee = getEmployeeList();


            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    productPicture.Picture = new byte[ImageFile.ContentLength];
                    ImageFile.InputStream.Read(productPicture.Picture, 0, ImageFile.ContentLength);
                }

                db.ProductPictures.Add(productPicture);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "StoreDetail");
            }
            return View(productPicture);
        }

        // GET: ProductPicture/Edit/5
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> Edit(int? id)
        {

            ViewBag.Employee = getEmployeeList();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductPicture productPicture = await db.ProductPictures.FindAsync(id);
            if (productPicture == null)
            {
                return HttpNotFound();
            }
            return View(productPicture);
        }

        // POST: ProductPicture/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> Edit([Bind(Include = "Picture_Id, employee_Id, Title, Picture, Price, Description")] ProductPicture productPicture, HttpPostedFileBase ImageFile)
        {

            ViewBag.Employee = getEmployeeList();

            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    productPicture.Picture = new byte[ImageFile.ContentLength];
                    ImageFile.InputStream.Read(productPicture.Picture, 0, ImageFile.ContentLength);
                }

                db.Entry(productPicture).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "StoreDetail");
            }
            return View(productPicture);
        }

        // GET: ProductPicture/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductPicture productPicture = await db.ProductPictures.FindAsync(id);
            if (productPicture == null)
            {
                return HttpNotFound();
            }
            var photo = (from pic in db.ProductPictures
                         where pic.Picture_Id == id
                         select new { pic.Picture }).ToList();
            foreach (var item in photo)
            {
                ViewBag.Photo = item.Picture;
            }
            return View(productPicture);
        }

        // POST: ProductPicture/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ProductPicture productPicture = await db.ProductPictures.FindAsync(id);
            db.ProductPictures.Remove(productPicture);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "StoreDetail");
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
    }
}
