using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Principal;
using UyumYemekSipariş.Models;

namespace UyumYemekSipariş.Controllers
{
    [Authorize]
    public class RestaurantOwnerController : Controller
    {
        UyumYemekSiparisDBEntities db = new UyumYemekSiparisDBEntities();

       
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Auth");
            }
            var email = User.Identity.Name;
            var user = db.Users.FirstOrDefault(u => u.Email == email);

            if (user != null)
            {
                ViewBag.FullName = user.FirstName + user.LastName; 
            }
            int ownerId = Convert.ToInt32(Session["UserID"]);
            var restaurant = db.Restaurants.FirstOrDefault(r => r.OwnerID == ownerId);

            if (restaurant == null)
            {
                ViewBag.Error = "Henüz restoranınız onaylanmamış veya kayıtlı değil.";
                ViewBag.HasRestaurant = false;
                return View();
            }

         
            return View();
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