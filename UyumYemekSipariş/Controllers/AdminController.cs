using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;
using UyumYemekSipariş.Models;

namespace UyumYemekSipariş.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        UyumYemekSiparisDBEntities db = new UyumYemekSiparisDBEntities();

       
        public ActionResult Index()
        {
           
            if (Session["UserID"] == null || Session["UserType"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Auth");
            }

           
            ViewBag.TotalRestaurants = db.Restaurants.Count();
            ViewBag.PendingRestaurants = db.Restaurants.Count(x => x.IsApproved == false);
            ViewBag.TotalUsers = db.Users.Count();
            ViewBag.TotalOrders = db.Orders.Count();
            ViewBag.TotalRevenue = db.Orders.Sum(x => x.TotalAmount) ?? 0;

            return View();
        }

      
        public ActionResult Restaurants()
        {
            if (Session["UserID"] == null || Session["UserType"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Auth");
            }

            var restaurants = db.Restaurants.Include("User").ToList();
            return View(restaurants);
        }

        // Restoran onaylama
        [HttpPost]
        public JsonResult ApproveRestaurant(int restaurantId, bool isApproved)
        {
           
                var restaurant = db.Restaurants.FirstOrDefault(x => x.RestaurantID == restaurantId);
                if (restaurant != null)
                {
                    restaurant.IsApproved = isApproved;
                    db.SaveChanges();
                    return Json(new { sonuc = 0 }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { sonuc = 1 }, JsonRequestBehavior.AllowGet);
           
        }

        // Restoran Aktif/Pasif durumuna getirme 
        [HttpPost]
        public JsonResult ToggleRestaurantStatus(int restaurantId, bool isActive)
        {
              var restaurant = db.Restaurants.FirstOrDefault(x => x.RestaurantID == restaurantId);
                if (restaurant != null)
                {
                    restaurant.IsActive = isActive;
                    db.SaveChanges();
                    return Json(new { sonuc = 0 }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { sonuc = 1 }, JsonRequestBehavior.AllowGet);
          
        }

      
        public ActionResult Categories()
        {
            if (Session["UserID"] == null || Session["UserType"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Auth");
            }

            var categories = db.Categories.ToList();
            return View(categories);
        }

        [HttpPost]
        public JsonResult DeleteCategory(int id)
        {
                var category = db.Categories.FirstOrDefault(x => x.CategoryID == id);
                if (category == null)
                {
                    return Json(new { sonuc = 1, mesaj = "Kategori bulunamadı!" });
                }

             //Kategoriye ait ürün olup olmadıgı
                var productCount = db.Products.Count(x => x.CategoryID == id);
                if (productCount > 0)
                {
                    return Json(new { sonuc = 1, mesaj = $"Bu kategoriye ait {productCount} ürün var! Önce ürünleri silin." });
                }

                // Kategoriyi sil
                db.Categories.Remove(category);
                db.SaveChanges();

                return Json(new { sonuc = 0, mesaj = "Kategori başarıyla silindi!" });
           
        }

        // Tüm Ürünler
        public ActionResult Products()
        {
            if (Session["UserID"] == null || Session["UserType"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Auth");
            }

            var products = db.Products.Include("Category").Include("Restaurant").ToList();
            return View(products);
        }

        // Tüm Siparişler
        public ActionResult Orders()
        {
            if (Session["UserID"] == null || Session["UserType"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Auth");
            }

            var orders = db.Orders.Include("User").Include("Restaurant").Include("OrderDetails.Product")
                                 .OrderByDescending(x => x.OrderDate)
                                 .ToList();
            return View(orders);
        }

        // Tüm Kullanıcılar
        public ActionResult Users()
        {
            if (Session["UserID"] == null || Session["UserType"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Auth");
            }

            var users = db.Users.ToList();
            return View(users);
        }

        // Kullanıcı Aktif/Pasif
     
        [HttpPost]
        public JsonResult ToggleUserStatus(int userId, bool isActive)
        {
                var user = db.Users.FirstOrDefault(x => x.UserID == userId);
                if (user == null)
                {
                    return Json(new { sonuc = 1, mesaj = "Kullanıcı bulunamadı!" });
                }

              
                string oncekiDurum = user.IsActive == true ? "Aktif" : "Pasif";
                string yeniDurum = isActive ? "Aktif" : "Pasif";

                user.IsActive = isActive;
                db.SaveChanges();

                return Json(new
                {
                    sonuc = 0,
                    mesaj = $"Kullanıcı durumu {oncekiDurum} → {yeniDurum} olarak değiştirildi!"
                });
           
        }

        // Kullanıcı Silme
        [HttpPost]
        public JsonResult DeleteUser(int id)
        {
            
                var user = db.Users.FirstOrDefault(x => x.UserID == id);
                if (user != null)
                {
                   
                    if (user.UserID == (int)Session["UserID"])
                    {
                        return Json(new { sonuc = 1, mesaj = "Kendinizi silemezsiniz!" });
                    }

                    db.Users.Remove(user);
                    db.SaveChanges();
                    return Json(new { sonuc = 0, mesaj = "Kullanıcı silindi!" });
                }
                return Json(new { sonuc = 1, mesaj = "Kullanıcı bulunamadı!" });
           
        }
    }
}