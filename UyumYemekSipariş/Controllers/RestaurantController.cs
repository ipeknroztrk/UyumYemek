using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UyumYemekSipariş.Models;

namespace UyumYemekSipariş.Controllers
{
    [Authorize]
    public class RestaurantController : Controller
    {
        UyumYemekSiparisDBEntities db = new UyumYemekSiparisDBEntities();
        public ActionResult Index()
        {
            var restoranlar = db.Restaurants.ToList();
            return View(restoranlar);
        }
        public ActionResult Details(int? id) 
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Default");
            }

            var restoran = db.Restaurants.FirstOrDefault(r => r.RestaurantID == id.Value);
            if (restoran == null)
            {
                return HttpNotFound();
            }

            var restoranUrunleri = db.Products.Where(p => p.RestaurantID == id.Value).ToList();
            ViewBag.Kategoriler = db.Categories.ToList();
            ViewBag.Restoran = restoran;

            return View(restoranUrunleri);
        }
    }
}