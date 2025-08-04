using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UyumYemekSipariş.Models;

namespace UyumYemekSipariş.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        UyumYemekSiparisDBEntities db = new UyumYemekSiparisDBEntities();
        public ActionResult Index()
        {
            var products = db.Products
               .Include("Category")
               .Include("Restaurant") 
               .ToList();

            ViewBag.Kategoriler = db.Categories.ToList();

            return View(products);
        }
    }
}