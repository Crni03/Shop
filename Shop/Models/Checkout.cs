using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class Checkout
    {
        public List<BasketItem> BasketItems { get; set; }
        public List<Discount> Discounts { get; set; }
        [Display(Name ="Card Number")]
        public string CardNumber { get; set; }
        [Display(Name = "Shipping adress")]
        public string Adress { get; set; }

        public static List<Discount> UsedDiscounts { get; set; }


        public virtual BasketItem BasketItem { get; set; }
        public virtual Discount Discount { get; set; }


        public decimal GetTotalPrice() {

            decimal totalPrice = 0;
            decimal totalDiscounts = 0;

            foreach (BasketItem item in BasketItems)
            {
                totalPrice += item.GetTotalPrice();
            }
            if (UsedDiscounts != null)
            {
                decimal subtractValue = 0;
                foreach (Discount item in UsedDiscounts)
                {
                    if (!item.CanConjunc)
                    {
                        if (item.Percent)
                        {
                            totalDiscounts = totalPrice * item.Amount / 100;
                        }
                        else
                        {
                            subtractValue = item.Amount;
                        }
                        break;
                    }
                    else
                    {
                        if (item.Percent)
                        {
                            totalDiscounts += totalPrice * item.Amount / 100;
                        }
                        else
                        {
                            subtractValue += item.Amount;
                        }
                    }
                }
                totalDiscounts += subtractValue;    
            }
            return totalPrice - totalDiscounts;
        } 
    }
}