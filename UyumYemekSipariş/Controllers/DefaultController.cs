using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UyumYemekSipariş.Controllers
{
    [Authorize]
    public class DefaultController : Controller
    {
       
        public ActionResult Index()
        {
            return View();
        }
    }
}