using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTML_UMA.Models;

namespace HTML_UMA.Controllers
{
    public class SupportController : Controller
    {
        DB_UMAEntities db = new DB_UMAEntities();
        // GET: Support
        public ActionResult FAQ()
        {
            return View();
        }
        public ActionResult Contact()

        {
            return View();
        }
        public ActionResult Law()
        {
            return View();
        }
        public ActionResult Badrequest()
        {
            return View();
        }
        public ActionResult Ourshop()
        {
            var infor = db.Information_Company.ToList();
            return View(infor);
        }
    }
}