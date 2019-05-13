using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace JAnet_ALlison_PHotography.Models
{
    public class Booking : IValidatableObject
    {
        [Key]
        public int booking_Id { get; set; }

        [Display(Name = "Employee: ")]
        [Required(ErrorMessage = "Your {0} is required")]
        public string employee_Id { get; set; }

        [Display(Name = "Customer Email: ")]
        public string UserName { get; set; }

        [Display(Name = "Customer Name: ")]
        public string Name { get; set; }

        [Display(Name = "Date: ")]
        [Required(ErrorMessage = "Your {0} is required")]
        [DataType(DataType.Date)]
        public DateTime dateTime { set; get; }

        [Display(Name = "Photo Type: ")]
        [Required(ErrorMessage = "Your {0} is required")]
        public string PhotoType { get; set; }

        [Display(Name = "Medium: ")]
        [Required(ErrorMessage = "Your {0} is required")]
        public string Medium { get; set; }

        [Display(Name = "Number Of People: ")]
        [Required(ErrorMessage = "Your {0} is required")]
        [Range(1, 1000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int NumPeople { get; set; }

        [Required(ErrorMessage = "Your {0} is required")]
        [Display(Name = "Photo Detail: ")]
        public string PhotoDetail { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            DateTime tempDate = DateTime.Now;
            if (tempDate >= dateTime)
            {
                yield return
                    new ValidationResult(errorMessage: "The booking date cannot be previous to the current date.", memberNames: new[] { "dateTime" });

            }
        }



    }

}