using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HTML_UMA.Models;
using HTML_UMA.Models.RoleUser;
using PagedList;

namespace HTML_UMA.Areas.Admin.Controllers
{
    [ValidRole(ValidRole = "adminstrator")]
    public class DonViController : Controller
    {
        private DB_UMAEntities db = new DB_UMAEntities();

        // GET: Admin/DonVi
        public ActionResult Index()
        {
            return View(db.OptionSales.ToList());
        }

        // GET: Admin/DonVi/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OptionSale optionSale = db.OptionSales.Find(id);
            if (optionSale == null)
            {
                return HttpNotFound();
            }
            return View(optionSale);
        }

        // GET: Admin/DonVi/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/DonVi/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OptionID,Name")] OptionSale optionSale)
        {
            if (ModelState.IsValid)
            {
                db.OptionSales.Add(optionSale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OptionID = new SelectList(db.OptionSales, "OptionID", "Name", optionSale.OptionID);
            ViewBag.OptionID = new SelectList(db.OptionSales, "OptionID", "Name", optionSale.OptionID);
            return View(optionSale);
        }

        // GET: Admin/DonVi/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OptionSale optionSale = db.OptionSales.Find(id);
            if (optionSale == null)
            {
                return HttpNotFound();
            }
            ViewBag.OptionID = new SelectList(db.OptionSales, "OptionID", "Name", optionSale.OptionID);
            ViewBag.OptionID = new SelectList(db.OptionSales, "OptionID", "Name", optionSale.OptionID);
            return View(optionSale);
        }

        // POST: Admin/DonVi/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OptionID,Name")] OptionSale optionSale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(optionSale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OptionID = new SelectList(db.OptionSales, "OptionID", "Name", optionSale.OptionID);
            ViewBag.OptionID = new SelectList(db.OptionSales, "OptionID", "Name", optionSale.OptionID);
            return View(optionSale);
        }

        // GET: Admin/DonVi/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OptionSale optionSale = db.OptionSales.Find(id);
            if (optionSale == null)
            {
                return HttpNotFound();
            }
            return View(optionSale);
        }

        // POST: Admin/DonVi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OptionSale optionSale = db.OptionSales.Find(id);
            db.OptionSales.Remove(optionSale);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult TimKiem(string search, int? Page)
        {
            int pageSize = 20;
            int pageNumber = (Page ?? 1);
            var item = db.OptionSales.Where(x => x.Name.Contains(search)).ToList();
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
