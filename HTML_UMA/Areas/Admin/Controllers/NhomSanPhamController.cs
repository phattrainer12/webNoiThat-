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
using HTML_UMA.Areas.Admin.Models;
using Newtonsoft.Json;

namespace HTML_UMA.Areas.Admin.Controllers
{
    [ValidRole(ValidRole = "adminstrator")]
    public class NhomSanPhamController : Controller
    {
        private DB_UMAEntities db = new DB_UMAEntities();

        // GET: Admin/NhomSanPham
        public ActionResult Index(int? Page)
        {
            int pageSize = 20;
            int pageNumber = (Page ?? 1);
            var item = db.GroupProducts.ToList();
            return View(item.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/NhomSanPham/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupProduct groupProduct = db.GroupProducts.Find(id);
            if (groupProduct == null)
            {
                return HttpNotFound();
            }
            return View(groupProduct);
        }

        // GET: Admin/NhomSanPham/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/NhomSanPham/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "group_ID,group_Name,image,Description")] GroupProduct groupProduct, HttpPostedFileBase Image)
        {
            if(ModelState.IsValid)
            {
                string path = Server.MapPath("~/Uploads/nhomsanpham");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filename = Image.FileName.Split('\\').Last();
                Image.SaveAs(path + "\\" + filename);
                groupProduct.image = "/Uploads/nhomsanpham" + "\\" + filename;
                db.GroupProducts.Add(groupProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(groupProduct);
        }

        // GET: Admin/NhomSanPham/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupProduct groupProduct = db.GroupProducts.Find(id);
            if (groupProduct == null)
            {
                return HttpNotFound();
            }
            return View(groupProduct);
        }

        // POST: Admin/NhomSanPham/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "group_ID,group_Name,image,Description")] GroupProduct groupProduct, HttpPostedFileBase ChangeImage)
        {
            if (ModelState.IsValid)
            {
                if (ChangeImage != null)
                {
                    string path = Server.MapPath("~/Uploads/nhomsanpham");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string filename = ChangeImage.FileName.Split('\\').Last();
                    ChangeImage.SaveAs(path + "\\" + filename);
                    groupProduct.image = "/Uploads/nhomsanpham" + "\\" + filename;
                }
                db.Entry(groupProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(groupProduct);
        }

        // GET: Admin/NhomSanPham/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupProduct groupProduct = db.GroupProducts.Find(id);
            if (groupProduct == null)
            {
                return HttpNotFound();
            }
            return View(groupProduct);
        }

        // POST: Admin/NhomSanPham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GroupProduct groupProduct = db.GroupProducts.Find(id);
            db.GroupProducts.Remove(groupProduct);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public string GroupSearch()
        {
            var Value = db.GroupProducts.ToList();
            var Res = new List<GroupSearch>();
            foreach (var item in Value)
            {
                GroupSearch Senditem = new GroupSearch();
                Senditem.ID = item.group_ID;
                Senditem.Name = item.group_Name;
                Senditem.Image = item.image;
                Senditem.label = item.group_Name;
                Res.Add(Senditem);
            }
            string json = JsonConvert.SerializeObject(Res);
            return json;
        }
        public ActionResult TimKiem(string search, int? Page)
        {
            int pageSize = 20;
            int pageNumber = (Page ?? 1);
            var item = db.GroupProducts.Where(x => x.group_Name.Contains(search)).ToList();
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
