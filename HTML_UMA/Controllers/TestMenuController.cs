using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HTML_UMA.Models;
namespace HTML_UMA.Controllers
{
    public class TestMenuController : Controller
    {
        private DB_UMAEntities db = new DB_UMAEntities();
        // GET: TestMenu
        public ActionResult Menu()
        {
            List<Menu> parent = db.Menus.Where(x => x.ParentIid == null).OrderBy(x=>x.Status).ToList();

            return View(parent);
        }
        public ActionResult childrentlv1(int parent_id)
        {
            List<Menu> child = db.Menus.Where(x => x.ParentIid == parent_id).OrderBy(x => x.Status).ToList();
            ViewBag.Count = child.Count();
            return View("childrentlv1", child);
        }
        public ActionResult childrentCategorylv2(int parent_id)
        {
            List<Menu> child = db.Menus.Where(x => x.ParentIid == parent_id).ToList();
            ViewBag.Count = child.Count();
            return View("childrentCategorylv2", child);
        }
        public ActionResult childrentRoomlv2(int parent_id)
        {
            List<Menu> child = db.Menus.Where(x => x.ParentIid == parent_id).ToList();
            ViewBag.Count = child.Count();
            return View("childrentRoomlv2", child);
        }
        public ActionResult MenuMobile()
        {
            List<Menu> parent = db.Menus.Where(x => x.ParentIid == null).ToList();

            return View(parent);
        }
        public ActionResult MenuMobilechildlv1(int parent_id)
        {
            List<Menu> child = db.Menus.Where(x => x.ParentIid == parent_id).ToList();
            ViewBag.Count = child.Count();
            return View("MenuMobilechildlv1", child);
        }
        public ActionResult MenuMobilechildRoomlv2(int parent_id)
        {
            List<Menu> child = db.Menus.Where(x => x.ParentIid == parent_id).ToList();
            ViewBag.Count = child.Count();
            return View("MenuMobilechildRoomlv2", child);
        }
        public ActionResult MenuMobilechildCategorylv2(int parent_id)
        {
            List<Menu> child = db.Menus.Where(x => x.ParentIid == parent_id).ToList();
            ViewBag.Count = child.Count();
            return View("MenuMobilechildCategorylv2", child);
        }
    }
}