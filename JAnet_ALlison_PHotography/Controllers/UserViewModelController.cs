﻿using JAnet_ALlison_PHotography.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Security;
using System.Security.Claims;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JAnet_ALlison_PHotography.Controllers
{
    public class UserViewModelController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        List<SelectListItem> list = new List<SelectListItem>(); // greate a list to populate with employee user
        List<UserViewModel> UserList = new List<UserViewModel>(); // to hold list of Users

        // GET: UserViewModel
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {

            var currentUser = System.Web.HttpContext.Current.User.Identity.GetUserName();
            ViewBag.UserId = currentUser;



            List<UserViewModel> modelLst = new List<UserViewModel>();
            var role = context.Roles.Include(x => x.Users).ToList();

            foreach (var r in role)
            {
                foreach (var u in r.Users)
                {
                    var usr = context.Users.Find(u.UserId);
                    var obj = new UserViewModel
                    {
                        Id = usr.Id,
                        FirstName = usr.FirstName,
                        LastName = usr.LastName,
                        RoleName = r.Name
                    };
                    modelLst.Add(obj);
                }
                //ViewBag.ModelList = modelLst;
            }

            return View(modelLst);

        }

    }
}