using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTML_UMA.Models;

namespace HTML_UMA.Controllers
{
    public class SliderController : Controller
    {
        private DB_UMAEntities db = new DB_UMAEntities();
        // GET: Slider
        public ActionResult Slider()
        {
            List<Slider> slider = db.Sliders.ToList();
            return View(slider);
        }
    }
}