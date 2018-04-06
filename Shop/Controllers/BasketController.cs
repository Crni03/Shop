using Microsoft.AspNet.Identity.Owin;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
    public class BasketController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        List<BasketItem> lstBasketItems = null;
        List<Discount> lstDiscounts = null;
        List<Product> lstProducts = null;

        [HttpGet]
        public ActionResult Index()
        {
            if (Session["BasketItems"] != null)
            {
                lstBasketItems = (List<BasketItem>)Session["BasketItems"];
                lstDiscounts = (List<Discount>)Session["DiscountsLeft"];
                lstProducts = (List<Product>)Session["Products"];
            }
            else
            {
                lstBasketItems = new List<BasketItem>();
                try
                {
                    lstDiscounts = db.Discounts.ToList();
                    lstProducts = db.Products.ToList();
                    Session["BasketItems"] = lstBasketItems;
                    Session["DiscountsLeft"] = lstDiscounts;
                    Session["Products"] = lstProducts;
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "An error accurred, please try again! Error: " + ex.Message;
                    lstProducts = new List<Product>();
                }
            }
            ViewBag.BasketItems = lstBasketItems;
            ViewBag.ProductsList = lstProducts;
            return View(new BasketItem());
        }

        [HttpGet]
        public ActionResult GetById(int? id, string actionType)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BasketItem basketItem = null;
            try
            {
                lstBasketItems = (List<BasketItem>)Session["BasketItems"];
                basketItem = lstBasketItems.Find(b => b.Product.IdProduct == id);
                if (basketItem == null)
                {
                    return HttpNotFound();

                }

            }
            catch (Exception ex)
            {
                TempData["Message"] = "An error occurred finding the product, " + ex.Message;
                return RedirectToAction("Index");
            }

            if (actionType.Equals("AddOne"))
            {

                basketItem.Quantity++;

            }
            else if (actionType.Equals("SubtractOne"))
            {
                basketItem.Quantity--;
                if (basketItem.Quantity < 1)
                {
                    lstBasketItems.Remove(basketItem);
                }
            }
            else if (actionType.Equals("Remove"))
            {
                lstBasketItems.Remove(basketItem);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProduct(int? id, int quantity)
        {

            if (id != null && id != 0)
            {
                quantity = (quantity < 1) ? 1 : quantity;

                lstBasketItems = (List<BasketItem>)Session["BasketItems"];
                BasketItem bi = lstBasketItems.Find(x => x.Product.IdProduct == id);
                if (bi != null)
                {
                    bi.Quantity += quantity;
                }
                else
                {
                    Product p = db.Products.Find(id);
                    BasketItem b = new BasketItem() { Quantity = quantity, Product = p };
                    lstBasketItems.Add(b);
                    Session["BasketItems"] = lstBasketItems;
                }
            }
            else
            {
                TempData["Message"] = "Could not retrieve the product, please try again";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Clear()
        {

            Session["BasketItems"] = null;
            Session["DiscountsLeft"] = null;
            Session["Products"] = null;
            TempData["Message"] = "Thank you for your purchase!";

            return RedirectToAction("Index");
        }
    }
}