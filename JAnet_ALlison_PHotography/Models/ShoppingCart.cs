using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JAnet_ALlison_PHotography.Models
{
    public class ShoppingCart
    {
        public ProductPicture Picture { get; set; }
        public int Quantity { get; set; }

        public ShoppingCart(ProductPicture picture, int quantity) {
            Picture = picture;
            Quantity = quantity;
        }
    }
}