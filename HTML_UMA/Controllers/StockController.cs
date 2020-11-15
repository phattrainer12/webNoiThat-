using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HTML_UMA.Controllers
{
    public class StockController : Controller
    {
        // GET: Stock
        [HttpPost]
        public ActionResult Location()
        {
            Session["Location"] = "Location";
            return RedirectToAction("Product","Index");
        }

    }
}