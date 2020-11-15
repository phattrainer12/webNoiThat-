using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTML_UMA.Models;

namespace HTML_UMA.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        private DB_UMAEntities db = new DB_UMAEntities();
        public ActionResult Menu()
        {
            List<Menu> parent = db.Menus.Where(x => x.ParentIid == null).ToList();

            return View(parent);
        }
        public ActionResult childrent(int parent_id)
        {
            List<Menu> child = db.Menus.Where(x => x.ParentIid == parent_id).ToList();
            ViewBag.Count = child.Count();
            return View("childrent", child);
        }
        public ActionResult Menuleft()
        {
            List<Menu> parent = db.Menus.Where(x => x.ParentIid == null).ToList();
            return View(parent);
        }
        public ActionResult childrentleft(int parent_id)
        {
            List<Menu> child = db.Menus.Where(x => x.ParentIid == parent_id).ToList();
            ViewBag.Count = child.Count();
            return View("childrentleft", child);
        }
        public ActionResult Menulv2()
        {
            List<Menu> parent = db.Menus.Where(x => x.ParentIid == 1).ToList();

            return View(parent);
        }
        public ActionResult childrentlv2(int parent_id)
        {
            List<Menu> child = db.Menus.Where(x => x.ParentIid == parent_id).ToList();
            ViewBag.Count = child.Count();
            return View("childrentlv2", child);
        }
        public ActionResult MenuRoomlv2()
        {
            List<Menu> parent = db.Menus.Where(x => x.ParentIid == 2).ToList();

            return View(parent);
        }
        public ActionResult MenuRoomchildlv2(int parent_id)
        {
            List<Menu> child = db.Menus.Where(x => x.ParentIid == parent_id).ToList();
            ViewBag.Count = child.Count();
            return View("MenuRoomchildlv2", child);
        }
    }
}