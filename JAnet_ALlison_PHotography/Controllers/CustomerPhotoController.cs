using JAnet_ALlison_PHotography.Models;
using JAnet_ALlison_PHotography.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JAnet_ALlison_PHotography.Controllers
{
    public class CustomerPhotoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CustomerPhoto
        [Authorize]
        public ActionResult Index()
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

            List<CustomerPhoto> CustomerPhotoList = new List<CustomerPhoto>(); // to hold list of ProductPicture and Customer details 
            var BookList = (from cust in db.Users
                            join pic in db.PurchasedPhotoes
                            on cust.Id equals pic.Customer_Id
                            select new
                            {
                                cust.FirstName,
                                cust.LastName,
                                cust.Email,
                                pic.PurchasedPic_Id,
                                pic.Title,
                                pic.Picture,
                            }).ToList();

            //query getting data from database from joining two tables and storing data in CustomerPhotoList 

            foreach (var item in BookList)

            {

                CustomerPhoto obj = new CustomerPhoto(); // ViewModel 
                obj.Customer_Email = item.Email;
                obj.Picture_Id = item.PurchasedPic_Id;
                obj.Customer_Name = item.FirstName + " " + item.LastName;
                obj.Title = item.Title;
                obj.Picture = item.Picture;

                CustomerPhotoList.Add(obj);

            }

            //Using foreach loop fill data from StoreDetailList to List<CustomerPhoto>. 
            ViewBag.ListPhoto=CustomerPhotoList;
            return View(CustomerPhotoList); //List of CustomerPhotoList 
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

        //Download file
        public FileResult DownloadPhoto(int? id)
        {


            var FileById = (from pic in db.PurchasedPhotoes
                            where pic.PurchasedPic_Id == id
                            select new
                            { pic.Title, pic.Picture }).ToList().FirstOrDefault();

            return File(FileById.Picture, "image/jpg", FileById.Title + ".jpg");

        }
    }
}