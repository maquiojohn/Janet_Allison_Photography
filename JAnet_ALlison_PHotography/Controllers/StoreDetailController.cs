using JAnet_ALlison_PHotography.Models;
using JAnet_ALlison_PHotography.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JAnet_ALlison_PHotography.Controllers
{
    public class StoreDetailController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StoreDetail
        public ActionResult Index()
        {
            List<StoreDetail> StoreDetailList = new List<StoreDetail>(); // to hold list of ProductPicture and Employee details 
            var BookList = (from emp in db.Users
                            join pic in db.ProductPictures
                            on emp.Id equals pic.employee_Id
                            select new
                            {
                                emp.FirstName,
                                emp.LastName,
                                pic.Picture_Id,
                                pic.Title,
                                pic.Picture,
                                pic.Description,
                                pic.Price
                            }).ToList();

            //query getting data from database from joining two tables and storing data in StoreDetailList 

            foreach (var item in BookList)

            {

                StoreDetail obj = new StoreDetail(); // ViewModel 
                obj.Picture_Id = item.Picture_Id;
                obj.Emp_Name = item.FirstName + " " + item.LastName;
                obj.Title = item.Title;
                obj.Picture = item.Picture;
                obj.Price = item.Price;
                obj.Description = item.Description;

                StoreDetailList.Add(obj);

            }

            //Using foreach loop fill data from StoreDetailList to List<StoreDetailList>. 

            return View(StoreDetailList); //List of StoreDetailList 
        }

    }
}