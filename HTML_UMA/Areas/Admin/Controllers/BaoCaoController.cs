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
using HTML_UMA.Models.RoleUser;
using HTML_UMA.ModelConfirm;

namespace HTML_UMA.Areas.Admin.Controllers
{
    [ValidRole(ValidRole = "adminstrator")]
    public class BaoCaoController : Controller
    {
        private DB_UMAEntities db = new DB_UMAEntities();

        // GET: Admin/BaoCao
        public ActionResult Index(int? Page)
        {
            int pageSize = 20;
            int pageNumber = (Page ?? 1);
            var item = db.OrderDetails.OrderBy(x=>x.detail_ID).ToList();
            return View(item.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/BaoCao/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var detail = db.Orders.Where(x => x.detail_ID == id && x.paid == true).ToList();
            ViewBag.Infordetail = db.OrderDetails.SingleOrDefault(x => x.detail_ID == id);
            if (detail == null)
            {
                return HttpNotFound();
            }
            string sql = "UPDATE Notify SET status = 'False' WHERE ID = '" +id + "'";
            var update = db.Database.ExecuteSqlCommand(sql);
            db.SaveChanges();
            return View(detail);
        }
        [HttpPost]
        public ActionResult Details(int detail_ID)
        {
            try
            {
                var itemorder = db.OrderDetails.SingleOrDefault(x => x.detail_ID == detail_ID);
                itemorder.@checked = true;
                var Orderpaid = db.Orders.Where(x => x.detail_ID == detail_ID).ToList();
                foreach (var item in Orderpaid)
                {
                    var completeProduct = db.Products.SingleOrDefault(x => x.pro_ID == item.pro_ID);
                    completeProduct.pro_Quanty -= item.cart_Quanty;
                    if (completeProduct.pro_Quanty < 1)
                    {
                        completeProduct.pro_Status = false;
                    }
                    db.SaveChanges();
                }
                ViewBag.resMess = Email.Send(detail_ID);
                db.SaveChanges();
                var detail = db.Orders.Where(x => x.detail_ID == detail_ID && x.paid == true).ToList();
                ViewBag.Infordetail = db.OrderDetails.SingleOrDefault(x => x.detail_ID == detail_ID);
                return View(detail);
            }
            catch (Exception ex)
            {
                ViewBag.resMess = "Đã có lỗi sảy ra!";
                var detail = db.Orders.Where(x => x.detail_ID == detail_ID && x.paid == true).ToList();
                ViewBag.Infordetail = db.OrderDetails.SingleOrDefault(x => x.detail_ID == detail_ID);
                return View(detail);
            }
        }
        // GET: Admin/BaoCao/Create
        public ActionResult Create()
        {
            ViewBag.cart_ID = new SelectList(db.Orders, "cart_ID", "pro_Name");
            ViewBag.ID = new SelectList(db.SaleCodes, "ID", "sale_code");
            ViewBag.user_ID = new SelectList(db.Users, "user_ID", "user_PassWord");
            return View();
        }

        // POST: Admin/BaoCao/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "detail_ID,user_ID,cart_ID,ID,detail_TotalBegin,detail_fee,detail_Totalmoney,detail_ShipName,detail_ShipLastName,detail_ShipPhone,detail_ShipEmail,detail_ShipProvince,detail_ShipDistrict,detail_ShipTown,detail_ShipStreet,detail_PayName,detail_PayLastName,detail_PayPhone,detail_PayEmail,detail_PayProvince,detail_PayDistrict,detail_PayTown,detail_PayStreet,detail_dateorder,detail_note,checked")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                db.OrderDetails.Add(orderDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cart_ID = new SelectList(db.Orders, "cart_ID", "pro_Name", orderDetail.cart_ID);
            ViewBag.ID = new SelectList(db.SaleCodes, "ID", "sale_code", orderDetail.ID);
            ViewBag.user_ID = new SelectList(db.Users, "user_ID", "user_PassWord", orderDetail.user_ID);
            return View(orderDetail);
        }

        // GET: Admin/BaoCao/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.cart_ID = new SelectList(db.Orders, "cart_ID", "pro_Name", orderDetail.cart_ID);
            ViewBag.ID = new SelectList(db.SaleCodes, "ID", "sale_code", orderDetail.ID);
            ViewBag.user_ID = new SelectList(db.Users, "user_ID", "user_PassWord", orderDetail.user_ID);
            return View(orderDetail);
        }

        // POST: Admin/BaoCao/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "detail_ID,user_ID,cart_ID,ID,detail_TotalBegin,detail_fee,detail_Totalmoney,detail_ShipName,detail_ShipLastName,detail_ShipPhone,detail_ShipEmail,detail_ShipProvince,detail_ShipDistrict,detail_ShipTown,detail_ShipStreet,detail_PayName,detail_PayLastName,detail_PayPhone,detail_PayEmail,detail_PayProvince,detail_PayDistrict,detail_PayTown,detail_PayStreet,detail_dateorder,detail_note,checked")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cart_ID = new SelectList(db.Orders, "cart_ID", "pro_Name", orderDetail.cart_ID);
            ViewBag.ID = new SelectList(db.SaleCodes, "ID", "sale_code", orderDetail.ID);
            ViewBag.user_ID = new SelectList(db.Users, "user_ID", "user_PassWord", orderDetail.user_ID);
            return View(orderDetail);
        }

        // GET: Admin/BaoCao/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            return View(orderDetail);
        }

        // POST: Admin/BaoCao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            db.OrderDetails.Remove(orderDetail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        public ActionResult TimKiem(int search, int? Page)
        {
            int pageSize = 20;
            int pageNumber = (Page ?? 1);
            var item = db.OrderDetails.Where(x => x.detail_ID == search).ToList();
            ViewBag.search = search;
            return View(item.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ConfigEmail()
        {
            return View(db.InformationMails.ToList());
        }
        public ActionResult EditEmail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InformationMail item = db.InformationMails.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
        [HttpPost]
        public ActionResult EditEmail(InformationMail mail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mail);
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
