using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JAnet_ALlison_PHotography.ViewModel
{
    public class CustomerZipFile
    {
        public int Picture_Id { get; set; }

        [Display(Name = "Customer: ")]
        [Required(ErrorMessage = "Your {0} is required")]
        public string Customer_Name { get; set; }

        [Display(Name = "Title: ")]
        [Required(ErrorMessage = "Your {0} is required")]
        public string Title { get; set; }

        [Display(Name = "Picture: ")]
        public byte[] Picture { get; set; }
    }
}