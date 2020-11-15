using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTML_UMA.Models;

namespace HTML_UMA.Controllers
{
    public class CallbackController : Controller
    {
        private DB_UMAEntities db = new DB_UMAEntities();
        // GET: Callback
        public ActionResult GetSearchValue(string search)
        {
            var item = db.Products.Where(x => x.pro_Name.Contains(search) | x.pro_Description.Contains(search) | x.pro_code.Contains(search) | x.Color.NameColor.Contains(search)).ToList();
            ViewBag.data = item;
            return View();
        }
    }
}