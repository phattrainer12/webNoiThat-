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
using HTML_UMA.ModelConfirm;
using Newtonsoft.Json;
using OfficeOpenXml;
using HTML_UMA.Areas.Admin.Models;

namespace HTML_UMA.Areas.Admin.Controllers
{
    [ValidRole(ValidRole = "adminstrator")]
    public class QuanLiSanPhamController : Controller
    {
        private DB_UMAEntities db = new DB_UMAEntities();

        // GET: Admin/QuanLiSanPham
        public ActionResult Index(int? Page,int? value)
        {
            
            int pageSize = 20;
            int pageNumber = (Page ?? 1);
            List<Product> item;
            if (value == null)
            {
                item = db.Products.OrderByDescending(x => x.pro_Date).ToList();
            }
            else
            {
                item = db.Products.Where(x => x.Menu_ID == value).OrderByDescending(x => x.pro_Date).ToList();
            }
            ViewBag.Menu_ID = db.Menus.Where(x => x.ParentIid != null).ToList();
            return View(item.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/QuanLiSanPham/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/QuanLiSanPham/Create
        public ActionResult Create()
        {
            ViewBag.Color_ID = new SelectList(db.Colors, "Color_ID", "NameColor");
            ViewBag.group_ID = new SelectList(db.GroupProducts, "group_ID", "group_Name");
            ViewBag.Menu_ID = new SelectList(db.Menus.Where(x=>x.ParentIid != null).ToList(), "Menu_ID", "Name");
            ViewBag.OptionID = new SelectList(db.OptionSales, "OptionID", "Name");
            ViewBag.OptionID = new SelectList(db.OptionSales, "OptionID", "Name");
            return View();
        }

        // POST: Admin/QuanLiSanPham/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, HttpPostedFileBase Image, HttpPostedFileBase[] moreimage)
        {
            if (ModelState.IsValid)
            {
                string path = Server.MapPath("~/Uploads/AnhSanPham");
                string pathMoreimage = Server.MapPath("~/Uploads/AnhChiTietSanPham");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filename = Image.FileName.Split('\\').Last();
                Image.SaveAs(path + "\\" + filename);
                product.pro_Image = "/Uploads/AnhSanPham" + "\\" + filename;
                product.pro_Status = true;
                product.pro_Date = DateTime.Now;
                product.sale = false;
                db.Products.Add(product);
                db.SaveChanges();
                int id = product.pro_ID;
                if(moreimage.FirstOrDefault() != null)
                {
                    foreach (var item in moreimage)
                    {
                        if (!Directory.Exists(pathMoreimage))
                        {
                            Directory.CreateDirectory(pathMoreimage);
                        }
                        string Selectfile = item.FileName.Split('\\').Last();
                        item.SaveAs(pathMoreimage + "\\" + Selectfile);
                        LinkImage smallimage = new LinkImage();
                        smallimage.pro_ID = id;
                        smallimage.link_ImageSmall = "/Uploads/AnhChiTietSanPham" + "\\" + Selectfile;
                        db.LinkImages.Add(smallimage);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Create");
            }

            ViewBag.Color_ID = new SelectList(db.Colors, "Color_ID", "NameColor", product.Color_ID);
            ViewBag.group_ID = new SelectList(db.GroupProducts, "group_ID", "group_Name", product.group_ID);
            ViewBag.Menu_ID = new SelectList(db.Menus.Where(x => x.ParentIid != null).ToList(), "Menu_ID", "Name", product.Menu_ID);
            ViewBag.OptionID = new SelectList(db.OptionSales, "OptionID", "Name", product.OptionID);
            ViewBag.OptionID = new SelectList(db.OptionSales, "OptionID", "Name", product.OptionID);
            return View(product);
        }
        // GET: Admin/QuanLiSanPham/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.SmallImage = db.LinkImages.Where(x => x.pro_ID == id).ToList();
            ViewBag.Color_ID = new SelectList(db.Colors, "Color_ID", "NameColor", product.Color_ID);
            ViewBag.group_ID = new SelectList(db.GroupProducts, "group_ID", "group_Name", product.group_ID);
            ViewBag.Menu_ID = new SelectList(db.Menus, "Menu_ID", "Name", product.Menu_ID);
            ViewBag.OptionID = new SelectList(db.OptionSales, "OptionID", "Name", product.OptionID);
            ViewBag.OptionID = new SelectList(db.OptionSales, "OptionID", "Name", product.OptionID);
            return View(product);
        }

        // POST: Admin/QuanLiSanPham/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, HttpPostedFileBase Image, Boolean DeleteSmallImage, HttpPostedFileBase[] moreimage)
        {
            if (ModelState.IsValid)
            {
                string path = Server.MapPath("~/Uploads/AnhSanPham");
                string pathMoreimage = Server.MapPath("~/Uploads/AnhChiTietSanPham");
                if (Image != null)
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string filename = Image.FileName.Split('\\').Last();
                    Image.SaveAs(path + "\\" + filename);
                    product.pro_Image = "/Uploads/AnhSanPham" + "\\" + filename;
                }
                if (DeleteSmallImage.Equals(true))
                {
                    var remove = db.LinkImages.Where(x => x.pro_ID == product.pro_ID).ToList();
                    foreach(var i in remove)
                    {
                        db.LinkImages.Remove(i);                    
                    }
                }
                if(moreimage.FirstOrDefault() != null)
                {
                    var deteleitem = db.LinkImages.Where(x => x.pro_ID == product.pro_ID).ToList();
                    foreach (var item in deteleitem)
                    {
                        db.LinkImages.Remove(item);
                    }
                        
                    foreach (var item in moreimage)
                    {
                        if (!Directory.Exists(pathMoreimage))
                        {
                            Directory.CreateDirectory(pathMoreimage);
                        }
                        string Selectfile = item.FileName.Split('\\').Last();
                        item.SaveAs(pathMoreimage + "\\" + Selectfile);
                        LinkImage smallimage = new LinkImage();
                        smallimage.pro_ID = product.pro_ID;
                        smallimage.link_ImageSmall = "/Uploads/AnhChiTietSanPham" + "\\" + Selectfile;
                        db.LinkImages.Add(smallimage);
                    }
                }
                
                db.Entry(product).State = EntityState.Modified;
                if(product.pro_Quanty > 0)
                {
                    product.pro_Status = true;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Color_ID = new SelectList(db.Colors, "Color_ID", "NameColor", product.Color_ID);
            ViewBag.group_ID = new SelectList(db.GroupProducts, "group_ID", "group_Name", product.group_ID);
            ViewBag.Menu_ID = new SelectList(db.Menus, "Menu_ID", "Name", product.Menu_ID);
            ViewBag.OptionID = new SelectList(db.OptionSales, "OptionID", "Name", product.OptionID);
            ViewBag.OptionID = new SelectList(db.OptionSales, "OptionID", "Name", product.OptionID);
            return View(product);
        }

        // GET: Admin/QuanLiSanPham/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/QuanLiSanPham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var delsmallimage = db.LinkImages.Where(x => x.pro_ID == id).ToList();
            foreach (var item in delsmallimage)
            {
                db.LinkImages.Remove(item);
            }
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Sale(int? dataid)
        {
            var Saleitem = db.Products.SingleOrDefault(x => x.pro_ID == dataid);
            if(Saleitem.sale == false)
            {
                Saleitem.sale = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                Saleitem.sale = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
        }
        public string ModelSearch()
        {
            var Value = db.Products.ToList();
            var Res = new List<Search>();
            foreach(var item in Value)
            {
                Search Senditem = new Search();
                Senditem.Id = item.pro_ID;
                Senditem.pro_Code = item.pro_code;
                Senditem.pro_Name = item.pro_Name;
                Senditem.pro_Image = item.pro_Image;
                Senditem.label = item.pro_code + " - " + item.pro_Name;
                Res.Add(Senditem);
            }
            string json = JsonConvert.SerializeObject(Res);
            return json;
        }
        public string Notify()
        {
            var value = from c in db.Notifies.Where(x => x.status == true)
                        select new
                        {
                            c.ID,
                            c.status
                        };
            string json = JsonConvert.SerializeObject(value);
            return json;
        }
        public ActionResult TimKiem(string search, int? Page)
        {
            int pageSize = 20;
            int pageNumber = (Page ?? 1);
            var item = db.Products.Where(x => x.pro_Name.Contains(search) || x.pro_code.Contains(search)).ToList();
            ViewBag.search = search;
            return View(item.ToPagedList(pageNumber, pageSize));
        }
        
        public ActionResult ImPortXLS()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ImPortXLS(HttpPostedFileBase[] image)
        {
            string path = Server.MapPath("~/Uploads/AnhSanPham");
            List<Product> list = new List<Product>();
            if (Request != null && image != null)
            {
                HttpPostedFileBase file = Request.Files["xlsFile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    using (ExcelPackage package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;

                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            try
                            {
                                var productitem = new Product();
                                productitem.pro_Image = workSheet.Cells[rowIterator, 1].Value.ToString();
                                productitem.pro_code = workSheet.Cells[rowIterator, 2].Value.ToString();
                                productitem.pro_Name = workSheet.Cells[rowIterator, 3].Value.ToString();
                                productitem.Menu_ID = int.Parse(workSheet.Cells[rowIterator, 4].Value.ToString());
                                productitem.pro_Size = "Dài " + workSheet.Cells[rowIterator, 5].Value.ToString()
                                                       + "cm ,Rộng " + workSheet.Cells[rowIterator, 6].Value.ToString()
                                                       + "cm ,Cao " + workSheet.Cells[rowIterator, 6].Value.ToString() + "cm";
                                productitem.pro_Materials = workSheet.Cells[rowIterator, 8].Value.ToString();
                                productitem.Color_ID = int.Parse(workSheet.Cells[rowIterator, 9].Value.ToString());
                                productitem.OptionID = int.Parse(workSheet.Cells[rowIterator, 10].Value.ToString());
                                productitem.pro_price = int.Parse(workSheet.Cells[rowIterator, 11].Value.ToString());
                                productitem.pro_Quanty = int.Parse(workSheet.Cells[rowIterator, 12].Value.ToString());
                                if(string.IsNullOrEmpty(workSheet.Cells[rowIterator, 13].Value.ToString()))
                                {
                                    productitem.pro_Source = null;
                                }
                                else
                                {
                                    productitem.pro_Source = workSheet.Cells[rowIterator, 13].Value.ToString();
                                }
                                
                                productitem.pro_Description = "SHOME";
                                if (string.IsNullOrEmpty(workSheet.Cells[rowIterator, 14].Value.ToString()))
                                {
                                    productitem.pro_Source = null;
                                }
                                else
                                {
                                    productitem.pro_Instruction = workSheet.Cells[rowIterator, 14].Value.ToString();
                                }
                                list.Add(productitem);
                            }
                            catch(Exception e)
                            {
                                var errormessage = "Đã có lỗi sảy ra trong quá trình thêm, vui lòng kiểm tra lại file tại line" + rowIterator + e;
                                ModelState.AddModelError("errorXSL", errormessage);
                                return View();
                            }
                            
                        }
                    }
                }
                if (!Directory.Exists(path)){
                    Directory.CreateDirectory(path);
                    foreach (var img in image)
                    {
                        string Selectfile = img.FileName.Split('\\').Last();
                        img.SaveAs(path + "\\" + Selectfile);
                    }
                }
                else
                {
                    foreach (var img in image)
                    {
                            string Selectfile = img.FileName.Split('\\').Last();
                            img.SaveAs(path + "\\" + Selectfile);
                    }
                }
            }
            foreach(var item in list)
            {
                db.Products.Add(item);
                db.SaveChanges();
            }
            return View();
        }
        [HttpPost]
        public string UpdateStatus(String updateStatus)
        {
            var result = JsonConvert.DeserializeObject<List<ModelUpdateStatus>>(updateStatus);
            string resMess = null;
            foreach (var item in result)
            {
                try
                {
                    string sql = "UPDATE Product SET fastProduct = '" + item.fastProduct + "', newProduct = '" + item.newProduct + "', sale = '" + item.sale + "' WHERE pro_ID = '" + item.Id + "'";
                    var update = db.Database.ExecuteSqlCommand(sql);
                    db.SaveChanges();
                    resMess = "Cập nhật thành công";
                }
                catch
                {
                    resMess = "Đã có lỗi sảy ra";
                }

            }
            return resMess;
        }
        public ActionResult DownloadFile()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "/media/TeamplateXLSX/";
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + "TemplateImport.xlsx");
            string fileName = "TemplateImport.xlsx";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
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
