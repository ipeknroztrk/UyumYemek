using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UyumYemekSipariş.Models;

namespace UyumYemekSipariş.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        UyumYemekSiparisDBEntities db = new UyumYemekSiparisDBEntities();

            public ActionResult Index()
            {
               
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Auth");
                }

                // Giriş yapan kullanıcının restoranını bulmak
                int userId = (int)Session["UserID"];
                var restaurant = db.Restaurants.FirstOrDefault(x => x.OwnerID == userId);

                if (restaurant != null)
                {
                   
                    var today = DateTime.Today;

                 
                    var dailyRevenue = db.Orders.Where(x => x.RestaurantID == restaurant.RestaurantID &&
                                                            x.OrderDate.HasValue &&
                                                            x.OrderDate.Value.Year == today.Year &&
                                                            x.OrderDate.Value.Month == today.Month &&
                                                            x.OrderDate.Value.Day == today.Day)
                                               .ToList()
                                               .Sum(x => x.TotalAmount) ?? 0;

                   
                    var totalOrders = db.Orders.Count(x => x.RestaurantID == restaurant.RestaurantID);

                  
                    var totalProducts = db.Products.Count(x => x.RestaurantID == restaurant.RestaurantID);

                   
                    var activeOrders = db.Orders.Count(x => x.RestaurantID == restaurant.RestaurantID &&
                                                           (x.Status == "Pending" || x.Status == "Confirmed"));

                   
                    var recentOrders = db.Orders.Include("User")
                                               .Where(x => x.RestaurantID == restaurant.RestaurantID)
                                               .OrderByDescending(x => x.OrderDate)
                                               .Take(5)
                                               .ToList();

                   
                    ViewBag.DailyRevenue = dailyRevenue;
                    ViewBag.TotalOrders = totalOrders;
                    ViewBag.TotalProducts = totalProducts;
                    ViewBag.ActiveOrders = activeOrders;
                    ViewBag.RecentOrders = recentOrders;
                    ViewBag.RestaurantName = restaurant.Name;
                }
                else
                {
                   
                    ViewBag.DailyRevenue = 0;
                    ViewBag.TotalOrders = 0;
                    ViewBag.TotalProducts = 0;
                    ViewBag.ActiveOrders = 0;
                    ViewBag.RecentOrders = new List<Order>();
                    ViewBag.RestaurantName = "Restoran Bulunamadı";
                }

                return View();
            }
        }
    }
