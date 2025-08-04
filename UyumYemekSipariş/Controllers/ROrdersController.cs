using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;
using UyumYemekSipariş.Models;

namespace UyumYemek.Controllers
{
    [Authorize]
    public class ROrdersController : Controller
    {
        UyumYemekSiparisDBEntities db = new UyumYemekSiparisDBEntities();

        public ActionResult Index()
        {
           
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

          
            int userId = (int)Session["UserID"];
            var restaurant = db.Restaurants.FirstOrDefault(x => x.OwnerID == userId);

            if (restaurant != null)
            {
                var orders = db.Orders.Include("User").Include("OrderDetails.Product")
                                     .Where(x => x.RestaurantID == restaurant.RestaurantID)
                                     .OrderByDescending(x => x.OrderDate)
                                     .ToList();
                return View(orders);
            }

            return View(new List<Order>());
        }

        public ActionResult Aktif()
        {
            
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

         
            int userId = (int)Session["UserID"];
            var restaurant = db.Restaurants.FirstOrDefault(x => x.OwnerID == userId);

            if (restaurant != null)
            {
                var orders = db.Orders.Include("User").Include("OrderDetails.Product")
                                     .Where(x => x.RestaurantID == restaurant.RestaurantID &&
                                                (x.Status == "Pending" || x.Status == "Confirmed"))
                                     .OrderByDescending(x => x.OrderDate)
                                     .ToList();
                return View("Index", orders); 
            }

            return View("Index", new List<Order>());
        }

        [HttpPost]
        public JsonResult Sil(int id)
        {
          
                Order o = db.Orders.FirstOrDefault(x => x.OrderID == id);
                if (o != null)
                {
                   
                    var details = db.OrderDetails.Where(x => x.OrderID == id).ToList();
                    foreach (var detail in details)
                    {
                        db.OrderDetails.Remove(detail);
                    }

                 
                    db.Orders.Remove(o);
                    db.SaveChanges();
                    return Json(new { sonuc = 0, mesaj = "Sipariş silindi!" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { sonuc = 1, mesaj = "Sipariş bulunamadı!" }, JsonRequestBehavior.AllowGet);
           
        }

        [HttpPost]
        public JsonResult DurumGuncelle(int orderId, string status)
        {
           
                var order = db.Orders.FirstOrDefault(x => x.OrderID == orderId);
                if (order != null)
                {
                    order.Status = status;
                    db.SaveChanges();
                    return Json(new { sonuc = 0 }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { sonuc = 1 }, JsonRequestBehavior.AllowGet);
           
        }
    }
}