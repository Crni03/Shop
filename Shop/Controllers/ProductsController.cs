﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Shop.Models
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        [HttpGet]
        public ActionResult Index()
        {
            List<Product> lstProducts = new List<Product>();
            try
            {
                lstProducts = db.Products.ToList();
            }
            catch (Exception ex)
            {
                TempData["Message"] = String.Format("Could not load list, error details: {0}", ex.Message);
            }
            return View(lstProducts);
        }

        [HttpGet]
        public ActionResult GetById(int? id, string view)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = null;
            try
            {
                product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = String.Format("Error happened, details: {0}", ex.Message);
                product = new Product();
            }

            return View(view, product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdProduct,Name,Price,Quantity,QuantityPrice")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                TempData["Message"] = "Product added";
                return RedirectToAction("Index");
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdProduct,Name,Price,QuantityForPrice,QuantityPrice")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = "Product edited";
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpGet, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            TempData["Message"] = "Product deleted";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
