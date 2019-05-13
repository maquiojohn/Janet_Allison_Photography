﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JAnet_ALlison_PHotography.Models
{
    public class OrderDetail
    {
        [Key, Column(Order = 0)]
        public int Receipt_Id { get; set; }

        [Key, Column(Order = 1)]
        [Display(Name = "First Name")]
        public int Picture_Id { get; set; }

        [Display(Name = "Title: ")]
        [Required(ErrorMessage = "Your {0} is required")]
        public string Title { get; set; }

        [Display(Name = "Picture: ")]
        public byte[] Picture { get; set; }

        [Display(Name = "Price: ")]
        [Range(1, 1000000000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double Price { get; set; }

        [Display(Name = "Description: ")]
        [Required(ErrorMessage = "Your {0} is required")]
        public string Description { get; set; }

        
    }

    
}