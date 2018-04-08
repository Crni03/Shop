using Microsoft.AspNet.Identity;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{

    [Authorize]
    public class CheckoutController : Controller
    {


        private ApplicationDbContext db = new ApplicationDbContext();
        private List<BasketItem> lstBasketItems = null;
        private List<Discount> discountsLeft = null;

        [HttpGet]
        public ActionResult Index()
        {
            Checkout checkout = null;


            if (Session["BasketItems"] != null || Session["DiscountsLeft"] != null)
            {
                try
                {
                    lstBasketItems = (List<BasketItem>)Session["BasketItems"];
                    if (lstBasketItems.Count < 1)
                    {
                        TempData["Message"] = "Basket empty, add items";
                        return RedirectToAction("Index", "Basket");
                    }
                    discountsLeft = (List<Discount>)Session["DiscountsLeft"];
                    string currentUserId = User.Identity.GetUserId();
                    ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                    string cardNumber = currentUser.CardNumber;
                    string adress = currentUser.Adress;
                    checkout = new Checkout()
                    { BasketItems = lstBasketItems, Discounts = discountsLeft, CardNumber = cardNumber, Adress = adress };
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "An error accurred, " + ex.Message;
                    return RedirectToAction("Index", "Basket");
                }
            }
            else
            {
                TempData["Message"] = "Something went wrong, please try again!";
                return RedirectToAction("Index", "Basket");
            }
            if (Checkout.UsedDiscounts == null)
            {
                Checkout.UsedDiscounts = new List<Discount>();
            }
            ViewBag.UsedDiscounts = Checkout.UsedDiscounts;
            return View(checkout);
        }

        [HttpPost]
        public ActionResult GetById(int? id, string actionType)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Discount discount = null;
            try
            {
                discountsLeft = (List<Discount>)Session["DiscountsLeft"];

                if (actionType.Equals("Add"))
                {

                    discount = discountsLeft.Find(d => d.IdDiscount == id);
                    if (discount == null)
                    {
                        return HttpNotFound();

                    }
                    discountsLeft.Remove(discount);
                    Checkout.UsedDiscounts.Add(discount);
                }

                else if (actionType.Equals("Remove"))
                {
                    discount = Checkout.UsedDiscounts.Find(d => d.IdDiscount == id);
                    if (discount == null)
                    {
                        return HttpNotFound();
                    }
                    discountsLeft.Add(discount);
                    Checkout.UsedDiscounts.Remove(discount);
                }
                Session["DiscountsLeft"] = discountsLeft;

            }
            catch (Exception ex)
            {
                TempData["Message"] = "An error occurred finding the product, " + ex.Message;
                return RedirectToAction("Index");
            }


            return RedirectToAction("Index");
        }
    }
}
