using System.Linq;
using System.Web.Mvc;
using HTML_UMA.Models;
using PagedList;
using System.Collections.Generic;

namespace HTML_UMA.Areas.Client.Models
{
    public class PhongController : Controller
    {
        // GET: Phong
        // GET: SanPham
        // GET: TestLoadAjax
        private DB_UMAEntities db = new DB_UMAEntities();
        public ActionResult DanhMuc()
        {
            List<Menu> room = db.Menus.Where(x => x.ParentIid == 2).ToList();
            return View(room);
        }
        public ActionResult DanhMucPhong(int? IDPhong)
        {
            if (IDPhong == null)
            {
                return RedirectToAction("BadRequest", "Support");
            }
            ViewBag.NameCategory = db.Menus.SingleOrDefault(x => x.Menu_ID == IDPhong);
            ViewBag.Categorychild = db.Menus.Where(x => x.ParentIid == IDPhong).ToList();
            return View();
        }
        public ActionResult AjaxLoading(int IDPhong, int? Page)
        {
            int pageSize = 9;
            int pageNumber = (Page ?? 1);
            var item = db.Products.Where(x => x.Menu_ID == IDPhong).ToList();
            return View(item.ToPagedList(pageNumber, pageSize));
        }
    }
}