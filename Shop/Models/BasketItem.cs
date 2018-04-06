using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class BasketItem
    {
      
        public int Quantity { get; set; }

        public virtual Product Product { get; set; }


        public decimal GetTotalPrice()
        {


            decimal totalPrice = 0;
            int quantityForPrice = Product.QuantityForPrice;
            decimal quantityPrice = Product.QuantityPrice;
            decimal singlePrice = Product.Price;

            if (quantityForPrice > 0)
            {
                totalPrice = (Quantity / quantityForPrice) * quantityPrice;
                totalPrice += (Quantity % quantityForPrice) * singlePrice;
            }
            else
            {
                totalPrice = singlePrice * Quantity;
            }
            return totalPrice;
        }
    }

}