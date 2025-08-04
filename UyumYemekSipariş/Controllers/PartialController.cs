using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UyumYemekSipariş.Models;

namespace UyumYemekSipariş.Controllers
{
    [Authorize]
    public class PartialController : Controller
    {
        UyumYemekSiparisDBEntities db = new UyumYemekSiparisDBEntities();
        public ActionResult KategoriListesi()
        {
            var kategoriler = db.Categories.ToList();
            return PartialView("_KategoriPartial", kategoriler); 
        }

        public ActionResult ÜrünListesi()
        {
            var tümürünler = db.Products
                .Include("Category")
                .Include("Restaurant")  
                .Take(10)  
                .ToList();

            ViewBag.Kategoriler = db.Categories.ToList();
            return PartialView("_ÜrünlerPartial", tümürünler);


        }
        public ActionResult RestoranListesi()
        {
            var restoranlar = db.Restaurants.ToList();
            return PartialView("_RestoranPartial", restoranlar);
        }
       

    }
}