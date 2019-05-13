using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JAnet_ALlison_PHotography.Models
{
    public class UserViewModel
    {
        public UserViewModel() { }

        public UserViewModel(ApplicationRole role, ApplicationUser user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            RoleName = role.Name;
        }

        public string Id { get; set; }
        [Display(Name = "First Name: ")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name: ")]
        public string LastName { get; set; }
        [Display(Name = "Role: ")]
        public string RoleName { get; set; }
    }
}