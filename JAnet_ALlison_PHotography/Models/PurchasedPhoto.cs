using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JAnet_ALlison_PHotography.Models
{
    public class PurchasedPhoto
    {
        [Key]
        public int PurchasedPic_Id { get; set; }

        [Display(Name = "Customer: ")]
        [Required(ErrorMessage = "Your {0} is required")]
        public string Customer_Id { get; set; }

        [Display(Name = "Title: ")]
        [Required(ErrorMessage = "Your {0} is required")]
        public string Title { get; set; }

        [Display(Name = "Picture: ")]
        public byte[] Picture { get; set; }

        [Display(Name = "Upload Date: ")]
        [Required(ErrorMessage = "Your {0} is required")]
        [DataType(DataType.Date)]
        public DateTime UploadDate { set; get; } = DateTime.Now;

        [Display(Name = "Date Taken: ")]
        [Required(ErrorMessage = "Your {0} is required")]
        [DataType(DataType.Date)]
        public DateTime DateTaken { set; get; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UploadDate < DateTaken)
            {
                yield return
                    new ValidationResult(errorMessage: "The date taken cannot exceed the current date.", memberNames: new[] { "DateTaken" });

            }
        }
    }
}