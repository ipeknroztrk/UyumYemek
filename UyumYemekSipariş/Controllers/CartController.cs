using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UyumYemekSipariş.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        public ActionResult Index()
        {
          
            ViewBag.CartData = TempData["CartData"];
            ViewBag.RestaurantName = TempData["RestaurantName"];
            return View();
        }
        [HttpPost]
        public ActionResult Index(string cartData, string restaurantName)
        {
            TempData["CartData"] = cartData;
            TempData["RestaurantName"] = restaurantName;

           
            ViewBag.CartData = cartData;

          
            return RedirectToAction("Index");
        }

    }
}