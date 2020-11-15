using System.Web.Mvc;

namespace HTML_UMA.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new { controller = "BaoCao|Contacts|DanhMuc|DonVi|MauSac|NhomSanPham|QuanLiSanPham" },
                new[] { "HTML_UMA.Areas.Admin.Controllers" }
            );
        }
    }
}