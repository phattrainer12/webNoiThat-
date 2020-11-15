using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HTML_UMA.Models;
using PagedList;
using System.IO;
using HTML_UMA.Models.RoleUser;

namespace HTML_UMA.Areas.Admin.Controllers
{
    [ValidRole(ValidRole = "adminstrator")]
    public class DanhMucController : Controller
    {
        private DB_UMAEntities db = new DB_UMAEntities();

        // GET: Admin/DanhMuc
        public ActionResult Index(int? Page)
        {
            int pageSize = 20;
            int pageNumber = (Page ?? 1);
            var item = db.Menus.Where(x=>x.ParentIid != null).ToList();
            return View(item.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/DanhMuc/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // GET: Admin/DanhMuc/Create
        public ActionResult Create()
        {
            ViewBag.Menu_ID = new SelectList(db.Menus, "Menu_ID", "Name");
            return View();
        }

        // POST: Admin/DanhMuc/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Menu_ID,Name,Image,Link,ParentIid,Note,DateCreate,DelFlg")] Menu menu, HttpPostedFileBase Image)
        {

            if (ModelState.IsValid)
            {
                string path = Server.MapPath("~/Uploads/danhmuc");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (Image != null)
                {
                    string filename = Image.FileName.Split('\\').Last();
                    Image.SaveAs(path + "\\" + filename);
                    menu.Image = "/Uploads/danhmuc" + "\\" + filename;
                }
                else
                {
                    var errormessage = "Vui lòng chọn file ảnh!";
                    ModelState.AddModelError("error", errormessage);
                }
                db.Menus.Add(menu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(menu);
        }

        // GET: Admin/DanhMuc/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            ViewBag.Menu_ID = new SelectList(db.Menus, "Menu_ID", "Name",menu.ParentIid);
            return View(menu);
        }

        // POST: Admin/DanhMuc/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Menu menu, HttpPostedFileBase ChangeImage)
        {
            if (ModelState.IsValid)
            {
                if(ChangeImage != null)
                {
                    string path = Server.MapPath("~/Uploads/danhmuc");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string filename = ChangeImage.FileName.Split('\\').Last();
                    ChangeImage.SaveAs(path + "\\" + filename);
                    menu.Image = "/Uploads/danhmuc" + "\\" + filename;
                }
                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menu);
        }

        // GET: Admin/DanhMuc/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // POST: Admin/DanhMuc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Menu menu = db.Menus.Find(id);
            db.Menus.Remove(menu);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult TimKiem(string search, int? Page)
        {
            int pageSize = 20;
            int pageNumber = (Page ?? 1);
            var item = db.Menus.Where(x => x.Name.Contains(search)).ToList();
            ViewBag.search = search;
            return View(item.ToPagedList(pageNumber, pageSize));
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
