using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;
using UyumYemekSipariş.Models;

namespace UyumYemek.Controllers
{
    [Authorize]
    public class RRestaurantController : Controller
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

            if (restaurant == null)
            {
               
                restaurant = new Restaurant { OwnerID = userId };
            }

            return View(restaurant);
        }

        [HttpGet]
        public ActionResult Edit()
        {
           
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

           
            int userId = (int)Session["UserID"];
            var restaurant = db.Restaurants.FirstOrDefault(x => x.OwnerID == userId);

            if (restaurant == null)
            {
                
                restaurant = new Restaurant { OwnerID = userId };
            }

            return View(restaurant);
        }

        [HttpPost]
        public ActionResult Edit(Restaurant r)
        {
         
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                int userId = (int)Session["UserID"];
                r.OwnerID = userId;

                if (r.RestaurantID == 0)
                {
                  
                    r.IsApproved = true; 
                    r.IsActive = true;  
                    db.Restaurants.Add(r);
                }
                else
                {
                   
                    db.Entry(r).State = System.Data.Entity.EntityState.Modified;
                }

                db.SaveChanges();
                ViewBag.Message = "Restoran bilgileri başarıyla kaydedildi!";
                return View(r);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Kaydetme işlemi başarısız: " + ex.Message;
                return View(r);
            }
        }

        [HttpPost]
        public JsonResult ToggleStatus(bool isActive)
        {
            
                int userId = (int)Session["UserID"];
                var restaurant = db.Restaurants.FirstOrDefault(x => x.OwnerID == userId);

                if (restaurant != null)
                {
                    restaurant.IsActive = isActive;
                    db.SaveChanges();
                    return Json(new { sonuc = 0 });
                }
                return Json(new { sonuc = 1 });
            
        }
    }
}