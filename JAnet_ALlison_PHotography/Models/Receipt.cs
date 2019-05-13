using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JAnet_ALlison_PHotography.Models
{
    public class Receipt
    {
        [Key]
        public int Receipt_Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Display(Name = "Receipt Date: ")]
        [Required(ErrorMessage = "Your {0} is required")]
        [DataType(DataType.Date)]
        public DateTime Receipt_Date { set; get; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}