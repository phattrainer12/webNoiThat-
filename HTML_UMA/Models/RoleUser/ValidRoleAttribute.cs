using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTML_UMA.Areas.Client.Models;
using System.Data.Common;
namespace HTML_UMA.Models.RoleUser
{
    public class ValidRoleAttribute : AuthorizeAttribute
    {
        private DB_UMAEntities db = new DB_UMAEntities();
        public string ValidRole{ get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // get informatio user from session
            User user = (User)HttpContext.Current.Session["User"];
            //checking User
            if (user != null)
            {
                //Lay thong tin role
                var role = user.role_ID;
                Role getrole = null;

                try
                {
                    getrole = db.Roles.Where(x => x.role_ID == role).First<Role>();

                    if(getrole != null && getrole.role_Name == "adminstrator")
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }
    }
}