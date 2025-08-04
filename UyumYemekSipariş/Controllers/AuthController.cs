using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UyumYemekSipariş.Models;

namespace UyumYemekSipariş.Controllers
{
    public class AuthController : Controller
    {
        UyumYemekSiparisDBEntities db = new UyumYemekSiparisDBEntities();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var user = db.Users.FirstOrDefault(u =>
                u.Email == email &&
                u.Password == password &&
                u.IsActive == true);

            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.Email, false);
              

              
                Session["UserID"] = user.UserID;
                Session["UserType"] = user.UserType;
                Session["UserName"] = user.FirstName + " " + user.LastName;
                Session["UserEmail"] = user.Email;

               
                switch (user.UserType)
                {
                    case "Admin":
                        return RedirectToAction("Index", "Admin");
                    case "RestaurantOwner":
                        return RedirectToAction("Index", "RestaurantOwner");
                    case "Customer":
                        return RedirectToAction("Index", "Default");
                    default:
                        ViewBag.Error = "Bilinmeyen kullanıcı tipi.";
                        return View();
                }
            }
            else
            {
                ViewBag.Error = "Geçersiz email veya şifre.";
                return View();
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }

        
    }
}