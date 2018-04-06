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
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; }
        [Display(Name = "Shipping adress")]
        public string Adress { get; set; }

        public static List<Discount> UsedDiscounts { get; set; }


        public virtual BasketItem BasketItem { get; set; }
        public virtual Discount Discount { get; set; }



        private decimal TotalPrice { get; set; }
        private decimal TotalDiscount { get; set; }


        public decimal GetTotalPrice()
        {
            decimal totalPrice = 0;

            foreach (BasketItem item in BasketItems)
            {
                totalPrice += item.GetTotalPrice();
            }
            totalPrice = decimal.Round(totalPrice, 2, MidpointRounding.AwayFromZero);
            TotalPrice = totalPrice;
            return totalPrice;
        }

        public decimal GetTotalDiscount()
        {
            decimal totalDiscount = 0;

            if (UsedDiscounts != null)
            {
                decimal subtractValue = 0;
                foreach (Discount item in UsedDiscounts)
                {
                    if (!item.CanConjunc)
                    {
                        if (item.Percent)
                        {
                            totalDiscount = TotalPrice * item.Amount / 100;
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
                            totalDiscount += TotalPrice * item.Amount / 100;
                        }
                        else
                        {
                            subtractValue += item.Amount;
                        }
                    }
                }
                totalDiscount += subtractValue;
            }
            totalDiscount = decimal.Round(totalDiscount, 2, MidpointRounding.AwayFromZero);
            TotalDiscount = totalDiscount;
            return totalDiscount;
        }

        public decimal GetPrice()
        {
            return TotalPrice - TotalDiscount;
        }
    }
}