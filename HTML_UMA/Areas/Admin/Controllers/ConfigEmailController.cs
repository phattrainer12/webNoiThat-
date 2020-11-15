using HTML_UMA.Models;
using HTML_UMA.Models.RoleUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HTML_UMA.Areas.Admin.Controllers
{
    [ValidRole(ValidRole = "adminstrator")] 
    public class ConfigEmailController : Controller
    {
        private DB_UMAEntities db = new DB_UMAEntities();
        // GET: Admin/ConfigEmail
        public ActionResult Email()
        {
            return View(db.InformationMails.ToList());
        }
    }
}