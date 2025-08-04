using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;
using UyumYemekSipariş.Models;

namespace UyumYemek.Controllers
{
    [Authorize]
    public class RCustomersController : Controller
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
                var customers = db.Users.Where(u => db.Orders.Any(o => o.User.UserID == u.UserID && o.RestaurantID == restaurant.RestaurantID))
                                       .Distinct()
                                       .ToList();
                return View(customers);
            }

            return View(new List<User>());
        }

       

        [HttpPost]
        public JsonResult Engelle(int userId, bool isActive)
        {
            try
            {
                var user = db.Users.FirstOrDefault(x => x.UserID == userId);
                if (user != null)
                {
                    user.IsActive = isActive;
                    db.SaveChanges();
                    return Json(new { sonuc = 0 }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { sonuc = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sonuc = 1 }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}