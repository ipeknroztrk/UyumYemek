using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace UyumYemekSipariş
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Application_AuthenticateRequest()
        {
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (HttpContext.Current.User.Identity is FormsIdentity)
                {
                    FormsIdentity identity = (FormsIdentity)HttpContext.Current.User.Identity;
                    FormsAuthenticationTicket ticket = identity.Ticket;

                    // DEBUG EKLE
                    System.Diagnostics.Debug.WriteLine("=== AUTH DEBUG ===");
                    System.Diagnostics.Debug.WriteLine("User Name: " + identity.Name);
                    System.Diagnostics.Debug.WriteLine("Ticket UserData: '" + ticket.UserData + "'");

                    // Tek rol varsa split etme
                    string[] roles = { ticket.UserData };
                    HttpContext.Current.User = new GenericPrincipal(identity, roles);

                    // DEBUG: Rol atandı mı?
                    System.Diagnostics.Debug.WriteLine("Assigned Roles: " + string.Join(",", roles));
                    System.Diagnostics.Debug.WriteLine("IsInRole RestaurantOwner: " + HttpContext.Current.User.IsInRole("RestaurantOwner"));
                    System.Diagnostics.Debug.WriteLine("==================");
                }
            }
        }

    }
}
