using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;
using UyumYemekSipariş.Models;



namespace UyumYemek.Controllers
{//restron sahibi paneli
    [Authorize]
    public class CategoriesController : Controller
    {
        UyumYemekSiparisDBEntities db = new UyumYemekSiparisDBEntities();

        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        [HttpGet]
        public ActionResult Ekle()
        {
            return View(new Category());
        }

        [HttpPost]
        public ActionResult Ekle(Category c)
        {
            db.Categories.AddOrUpdate(c);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Sil(int id)
        {
            try
            {
                Category c = db.Categories.FirstOrDefault(x => x.CategoryID == id);
                if (c != null)
                {
                    db.Categories.Remove(c);
                    db.SaveChanges();
                    return Json(new { sonuc = 0, mesaj = "Kategori silindi!" });
                }
                return Json(new { sonuc = 1, mesaj = "Kategori bulunamadı!" });
            }
            catch (Exception ex)
            {
                return Json(new { sonuc = 1, mesaj = "Silme işlemi başarısız!" });
            }
        }

        [HttpPost]
        public JsonResult Guncelle(Category c)
        {
            var mevcut = db.Categories.FirstOrDefault(x => x.CategoryID == c.CategoryID);
            mevcut.Name = c.Name;
            mevcut.Description = c.Description;
            mevcut.IconUrl = c.IconUrl;
            db.SaveChanges();
            return Json(new { sonuc = 0 });
        }
    }
}