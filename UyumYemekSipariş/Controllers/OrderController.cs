using System;
using System.Linq;
using System.Web.Mvc;
using UyumYemekSipariş.Models;

namespace UyumYemekSipariş.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        UyumYemekSiparisDBEntities db = new UyumYemekSiparisDBEntities();

      
        public ActionResult Index()
        {
           
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            int kullaniciId = (int)Session["UserID"];

           
            var kullaniciSiparisleri = db.Orders
                .Where(o => o.CustomerID == kullaniciId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

          
            foreach (var siparis in kullaniciSiparisleri)
            {
                var restoran = db.Restaurants.Find(siparis.RestaurantID);
                if (restoran != null)
                {
                    siparis.Notes = restoran.Name; 
                }
            }

         
            var kullanici = db.Users.Find(kullaniciId);
            ViewBag.KullaniciAdi = kullanici != null ? kullanici.FirstName + " " + kullanici.LastName : "Kullanıcı";

            return View(kullaniciSiparisleri);
        }

       
        public ActionResult Details(int? id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!id.HasValue)
            {
                TempData["Error"] = "Geçersiz sipariş ID'si.";
                return RedirectToAction("Index");
            }

            int kullaniciId = (int)Session["UserID"];
            int siparisId = id.Value;

          
            var siparis = db.Orders
                .Where(o => o.OrderID == siparisId && o.CustomerID == kullaniciId)
                .FirstOrDefault();

            if (siparis == null)
            {
                TempData["Error"] = "Sipariş bulunamadı veya erişim yetkiniz yok.";
                return RedirectToAction("Index");
            }

           
            var siparisDetaylari = db.OrderDetails
                .Where(od => od.OrderID == siparisId)
                .ToList();

          
            foreach (var detay in siparisDetaylari)
            {
                var urun = db.Products.Find(detay.ProductID);
                if (urun != null)
                {
                   
                    detay.Product = urun;
                }
            }

          
            var restoran = db.Restaurants.Find(siparis.RestaurantID);
            ViewBag.RestoranAdi = restoran != null ? restoran.Name : "Bilinmiyor";

            ViewBag.SiparisDetaylari = siparisDetaylari;

            return View(siparis);
        }

       
        [HttpPost]
        public ActionResult SiparisIptal(int siparisId)
        {
            if (Session["UserID"] == null)
            {
                return Json(new { success = false, message = "Giriş yapmanız gerekiyor." });
            }

            int kullaniciId = (int)Session["UserID"];

            var siparis = db.Orders
                .Where(o => o.OrderID == siparisId && o.CustomerID == kullaniciId)
                .FirstOrDefault();

            if (siparis == null)
            {
                return Json(new { success = false, message = "Sipariş bulunamadı." });
            }

           
            if (siparis.Status.ToLower() != "pending")
            {
                return Json(new { success = false, message = "Bu sipariş artık iptal edilemez." });
            }

            try
            {
                siparis.Status = "Cancelled";
                db.SaveChanges();
                return Json(new { success = true, message = "Sipariş başarıyla iptal edildi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "İptal işlemi sırasında hata oluştu." });
            }
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