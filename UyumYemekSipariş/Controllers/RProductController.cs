using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UyumYemekSipariş.Models;

namespace UyumYemekSipariş.Controllers
{
    [Authorize]
        public class RProductController : Controller
        {
            UyumYemekSiparisDBEntities db = new UyumYemekSiparisDBEntities();

        public ActionResult Index()
        {
           
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

          
            ViewBag.Categories = db.Categories.ToList();

            int userId = (int)Session["UserID"];
            var restaurant = db.Restaurants.FirstOrDefault(x => x.OwnerID == userId);

            if (restaurant != null)
            {
                var products = db.Products.Include("Category")
                         .Where(x => x.RestaurantID == restaurant.RestaurantID)
                         .ToList();
                return View(products);
            }

            return View(new List<Product>());
        }

        [HttpGet]
        public ActionResult Ekle(int? id)
        {
           
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

          
            ViewBag.Categories = db.Categories.ToList();

            if (id == null)
            {
                return View(new Product());
            }
            else
            {
                // Güncelleme için
                var product = db.Products.Find(id);
                if (product == null)
                    return HttpNotFound();

                return View(product);
            }
        }

        [HttpPost]
        public ActionResult Ekle(Product p)
        {
           
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

          
            int userId = (int)Session["UserID"];
            var restaurant = db.Restaurants.FirstOrDefault(x => x.OwnerID == userId);

            if (restaurant != null)
            {
                p.RestaurantID = restaurant.RestaurantID;

                db.Products.AddOrUpdate(p);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Sil(int id)
        {
             Product p = db.Products.FirstOrDefault(x => x.ProductID == id);
                if (p != null)
                {
                    db.Products.Remove(p);
                    db.SaveChanges();
                    return Json(new { sonuc = 0, mesaj = "Ürün silindi!" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { sonuc = 1, mesaj = "Ürün bulunamadı!" }, JsonRequestBehavior.AllowGet);
            
        }

        [HttpPost]
        public JsonResult Guncelle(Product p)
        {
            var mevcut = db.Products.FirstOrDefault(x => x.ProductID == p.ProductID);
                if (mevcut != null)
                {
                    mevcut.Name = p.Name;
                    mevcut.Description = p.Description;
                    mevcut.Price = p.Price;
                    mevcut.CategoryID = p.CategoryID;
                    mevcut.ImagePath = p.ImagePath;
                    mevcut.IsAvailable = p.IsAvailable;
                    db.SaveChanges();
                    return Json(new { sonuc = 0 });
                }
                return Json(new { sonuc = 1 });
           
        }
    }
    }