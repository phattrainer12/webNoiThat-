using System.Linq;
using System.Web.Mvc;
using HTML_UMA.Models;
using PagedList;
using System.Collections.Generic;
using System;

namespace HTML_UMA.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        private DB_UMAEntities db = new DB_UMAEntities();
        public ActionResult DanhMuc()
        {
            List<Menu> menu = db.Menus.Where(x => x.ParentIid == 1).ToList();
            return View(menu);
        }
        public ActionResult DanhMucSanPham(int? IDDanhMuc, int? Page, string Orderby, int? Color, int? Begin,int? End)
        {
            if(IDDanhMuc == null)
            {
                return RedirectToAction("DanhMuc","SanPham");
            }
            int pageSize = 18;
            int pageNumber = (Page ?? 1);
            int ColorID = (Color ?? 0);
            int beginprice = Begin ?? 0;
            int endprice = End ?? 500000000;
            var item = db.Products.Where(x => x.Menu_ID == IDDanhMuc).ToList();
            switch (Orderby)
            {
                case "desc":
                    item = item.OrderByDescending(x => x.pro_price).ToList();
                    break;
                case "asc":
                    item = item.OrderBy(x => x.pro_price).ToList();
                    break;
            }
            if (ColorID != 0)
            {
                item = item.Where(x => x.Color_ID == ColorID).ToList();
            }
            if(beginprice != 0 || endprice != 0)
            {
                item = item.Where(x => x.pro_price > beginprice && x.pro_price < endprice).ToList();
            }
            ViewBag.Orderby = Orderby;
            ViewBag.Color = ColorID;
            ViewBag.Page = pageNumber;
            ViewBag.Begin = beginprice;
            ViewBag.End = endprice;
            ViewBag.NameCategory = db.Menus.SingleOrDefault(x => x.Menu_ID == IDDanhMuc);
            ViewBag.Categorychild = db.Menus.Where(x => x.ParentIid == IDDanhMuc).ToList();
            return View(item.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult HangMoiVe(int? Page, string Orderby, int? Color, int? Begin, int? End)
        {
            int pageSize = 18;
            int pageNumber = (Page ?? 1);
            int ColorID = (Color ?? 0);
            int beginprice = Begin ?? 0;
            int endprice = End ?? 500000000;
            DateTime aDateTime = DateTime.Now;
            DateTime newTime = aDateTime.AddMonths(-1);
            var item = db.Products.Where(x=>x.newProduct == true).ToList();
            switch (Orderby)
            {
                case "desc":
                    item = item.OrderByDescending(x => x.pro_price).ToList();
                    break;
                case "asc":
                    item = item.OrderBy(x => x.pro_price).ToList();
                    break;
            }
            if (ColorID != 0)
            {
                item = item.Where(x => x.Color_ID == ColorID).ToList();
            }
            if (beginprice != 0 || endprice != 0)
            {
                item = item.Where(x => x.pro_price > beginprice && x.pro_price < endprice).ToList();
            }
            ViewBag.Orderby = Orderby;
            ViewBag.Color = ColorID;
            ViewBag.Page = pageNumber;
            ViewBag.Begin = beginprice;
            ViewBag.End = endprice;
            return View(item.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult HangThanhLi(int? Page, string Orderby, int? Color, int? Begin, int? End)
        {
            int pageSize = 18;
            int pageNumber = (Page ?? 1);
            int ColorID = (Color ?? 0);
            int beginprice = Begin ?? 0;
            int endprice = End ?? 500000000;
            var item = db.Products.Where(x => x.sale == true).ToList();
            switch (Orderby)
            {
                case "desc":
                    item = item.OrderByDescending(x => x.pro_price).ToList();
                    break;
                case "asc":
                    item = item.OrderBy(x => x.pro_price).ToList();
                    break;
            }
            if (ColorID != 0)
            {
                item = item.Where(x => x.Color_ID == ColorID).ToList();
            }
            if (beginprice != 0 || endprice != 0)
            {
                item = item.Where(x => x.pro_price > beginprice && x.pro_price < endprice).ToList();
            }
            ViewBag.Orderby = Orderby;
            ViewBag.Color = ColorID;
            ViewBag.Page = pageNumber;
            ViewBag.Begin = beginprice;
            ViewBag.End = endprice;
            return View(item.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult BanChayNhat(int? Page, string Orderby, int? Color, int? Begin, int? End)
        {
            int pageSize = 18;
            int pageNumber = (Page ?? 1);
            int ColorID = (Color ?? 0);
            int beginprice = Begin ?? 0;
            int endprice = End ?? 500000000;
            var item = db.Products.Where(x => x.fastProduct == true).ToList();
            switch (Orderby)
            {
                case "desc":
                    item = item.OrderByDescending(x => x.pro_price).ToList();
                    break;
                case "asc":
                    item = item.OrderBy(x => x.pro_price).ToList();
                    break;
            }
            if (ColorID != 0)
            {
                item = item.Where(x => x.Color_ID == ColorID).ToList();
            }
            if (beginprice != 0 || endprice != 0)
            {
                item = item.Where(x => x.pro_price > beginprice && x.pro_price < endprice).ToList();
            }
            ViewBag.Orderby = Orderby;
            ViewBag.Color = ColorID;
            ViewBag.Page = pageNumber;
            ViewBag.Begin = beginprice;
            ViewBag.End = endprice;
            return View(item.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult BoSuuTapSanPham(int? IDBoSuuTap)
        {
            if (IDBoSuuTap == null)
            {
                return RedirectToAction("BadRequest", "Support");
            }
            ViewBag.NameGroup = db.GroupProducts.SingleOrDefault(x => x.group_ID == IDBoSuuTap);
            return View();
        }
        public ActionResult LoadBoSuuTapSanPham(int? IDBoSuuTap, int? Page)
        {
            int pageSize = 9;
            int pageNumber = (Page ?? 1);
            var item = db.Products.Where(x => x.group_ID == IDBoSuuTap).ToList();
            ViewBag.NameGroup = db.GroupProducts.SingleOrDefault(x => x.group_ID == IDBoSuuTap);
            return View(item.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ChiTietSanPham(int? MaSanPham)
        {
            if (MaSanPham == null)
            {
                return RedirectToAction("BadRequest", "Support");
            }
            var item = db.Products.SingleOrDefault(x => x.pro_ID == MaSanPham);
            if (item == null)
            {
                return RedirectToAction("BadRequest", "Support");
            }
            else
            {
                ViewBag.SmallImage = db.LinkImages.Where(x => x.pro_ID == MaSanPham).ToList();
                return View(item);
            }
        }
    }
}