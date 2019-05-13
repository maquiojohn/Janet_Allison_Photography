using JAnet_ALlison_PHotography.Models;
using JAnet_ALlison_PHotography.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JAnet_ALlison_PHotography.Controllers
{
    public class CustomerZipFileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CustomerZipFile
        [Authorize(Roles = "Admin, Employee")]
        public ActionResult Index()
        {
            List<CustomerZipFile> CustomerZipFileList = new List<CustomerZipFile>(); // to hold list of ProductPicture and Customer details 
            var BookList = (from cust in db.Users
                            join pic in db.ZipFiles
                            on cust.Id equals pic.Customer_Id
                            select new
                            {
                                cust.FirstName,
                                cust.LastName,
                                pic.PurchasedPic_Id,
                                pic.Title,
                                pic.Picture,
                            }).ToList();

            //query getting data from database from joining two tables and storing data in CustomerZipFileList 

            foreach (var item in BookList)

            {

                CustomerZipFile obj = new CustomerZipFile(); // ViewModel 
                obj.Picture_Id = item.PurchasedPic_Id;
                obj.Customer_Name = item.FirstName + " " + item.LastName;
                obj.Title = item.Title;
                obj.Picture = item.Picture;

                CustomerZipFileList.Add(obj);

            }

            //Using foreach loop fill data from StoreDetailList to List<CustomerZipFile>. 

            return View(CustomerZipFileList); //List of CustomerZipFileList 
        }
    }
}