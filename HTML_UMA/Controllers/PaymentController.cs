using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Facebook;
using HTML_UMA.Models;
using HTML_UMA.ModelConfirm;
using System.Configuration;
using System.Web.Security;
using System.Net;
namespace HTML_UMA.Areas.Client.Models
{

    public class PaymentController : Controller
    {
        public string CarstDetailSesstion = "CarstDetail";
        public string CartSesstion = "CarstSesstion";
        private DB_UMAEntities db = new DB_UMAEntities();
        // GET: Payment
        public ActionResult Onepage()
        {
            return View();
        }
        public ActionResult step1()
        {
            var user = Session["User"] as User;
            ViewBag.datauser = user;
                if (user != null)
                {
                    var itemview = db.Orders.Where(x => x.user_ID == user.user_ID && x.paid == false).ToList();
                    if(itemview.Count != 0)
                    {
                        ViewBag.cart = itemview;
                        return View(ViewBag.cart);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Product");
                    }
                    
                }
                else
                {
                    var cartAnonymous = Session[CarstDetailSesstion] as List<Order>;
                    if(cartAnonymous != null)
                    {
                        ViewBag.cartano = cartAnonymous.ToList();
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Product");
                    }

                }
            

        }
        [HttpPost]
        public ActionResult step1(string Email, string PassWord)
        {
            var Cartproduct = Session[CarstDetailSesstion] as List<Order>;
            if (Email != null && PassWord != null)
            {
                var user = db.Users.SingleOrDefault(x => x.user_Email == Email && x.user_PassWord == PassWord && x.type_register == 1);
                if (user != null)
                {
                    if (Cartproduct != null)
                    {
                        foreach (var item in Cartproduct)
                        {
                            item.user_ID = user.user_ID;
                            item.paid = false;
                            db.Orders.Add(item);
                            db.SaveChanges();
                        }
                        Session.Contents.Remove(CarstDetailSesstion);
                    }
                    if (user.role_ID == 1)
                    {
                        Session["User"] = user;
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        if (user.role_ID == 2)
                        {
                            Session["User"] = user;
                            return RedirectToAction("Index", "Admin");
                        }
                        else
                        {
                            Session["User"] = user;
                            return RedirectToAction("Index", "Product");
                        }
                    }
                }
                else
                {
                    var cartAnonymous = Session[CarstDetailSesstion] as List<Order>;
                    ViewBag.cartano = cartAnonymous;
                    var errormessage = "Tài khoản đăng nhập hoặc mật khẩu sai!";
                    ModelState.AddModelError("error", errormessage);
                    return View();
                }
            }
            return View();
        }
        public ActionResult step2()
        {
            try
            {
                var user = Session["User"] as User;
                ViewBag.datauser = user;
                if (user != null)
                {
                    var itemview = db.Orders.Where(x => x.user_ID == user.user_ID && x.paid == false).ToList();
                    ViewBag.cart = itemview;
                    return View(ViewBag.cart);
                }
                else
                {
                    var cartAnonymous = Session[CarstDetailSesstion] as List<Order>;
                    ViewBag.cartano = cartAnonymous;
                    return View(ViewBag.cartano);
                }
            }
            catch
            {
                return RedirectToAction("BadRequest", "Support");
            }
        }
        [HttpPost]
        public ActionResult ShippingInformation([Bind(Include = "detail_ShipName,detail_ShipLastName,detail_ShipPhone,detail_ShipEmail,detail_ShipProvince,detail_ShipDistrict,detail_ShipTown,detail_ShipStreet,detail_PayName,detail_PayLastName,detail_PayPhone,detail_PayEmail,detail_PayProvince,detail_PayDistrict,detail_PayTown,detail_PayStreet")]OrderDetailValid orderDetailValid)
        {
            var user = Session["User"] as User;
            var NewOrderDetail = new OrderDetail();
            decimal BeginPrice = 0;
            NewOrderDetail.@checked = false;

            try
            {

                if (orderDetailValid.detail_PayName != null && orderDetailValid.detail_PayLastName != null && orderDetailValid.detail_PayPhone != null && orderDetailValid.detail_PayEmail != null && orderDetailValid.detail_PayProvince != null
               && orderDetailValid.detail_PayDistrict != null && orderDetailValid.detail_PayTown != null && orderDetailValid.detail_PayStreet != null)
                {
                    NewOrderDetail.detail_PayName = orderDetailValid.detail_PayName;
                    NewOrderDetail.detail_PayLastName = orderDetailValid.detail_PayLastName;
                    NewOrderDetail.detail_PayPhone = orderDetailValid.detail_PayPhone;
                    NewOrderDetail.detail_PayEmail = orderDetailValid.detail_PayEmail;
                    NewOrderDetail.detail_PayProvince = orderDetailValid.detail_PayProvince;
                    NewOrderDetail.detail_PayDistrict = orderDetailValid.detail_PayDistrict;
                    NewOrderDetail.detail_PayTown = orderDetailValid.detail_PayTown;
                    NewOrderDetail.detail_PayStreet = orderDetailValid.detail_PayStreet;
                    NewOrderDetail.detail_ShipLastName = orderDetailValid.detail_ShipLastName;
                    NewOrderDetail.detail_ShipName = orderDetailValid.detail_ShipName;
                    NewOrderDetail.detail_ShipEmail = orderDetailValid.detail_ShipEmail;
                    NewOrderDetail.detail_ShipPhone = orderDetailValid.detail_ShipPhone;
                    NewOrderDetail.detail_ShipProvince = orderDetailValid.detail_ShipProvince;
                    NewOrderDetail.detail_ShipDistrict = orderDetailValid.detail_ShipDistrict;
                    NewOrderDetail.detail_ShipTown = orderDetailValid.detail_ShipTown;
                    NewOrderDetail.detail_ShipStreet = orderDetailValid.detail_ShipStreet;
                    if (user != null)
                    {
                        var itemview = db.Orders.Where(x => x.user_ID == user.user_ID && x.paid == false).ToList();
                        foreach (var item in itemview)
                        {
                            BeginPrice = Convert.ToDecimal(BeginPrice + item.cart_TotalPrice);
                        }
                        NewOrderDetail.user_ID = user.user_ID;
                        NewOrderDetail.detail_TotalBegin = BeginPrice;
                        Session["DetailOrder"] = NewOrderDetail;
                        return RedirectToAction("Step3");
                    }
                    else
                    {
                        var cartAnonymous = Session[CarstDetailSesstion] as List<Order>;
                        foreach (var item in cartAnonymous)
                        {
                            BeginPrice = Convert.ToDecimal(BeginPrice + item.cart_TotalPrice);
                        }
                        NewOrderDetail.detail_TotalBegin = BeginPrice;
                        NewOrderDetail.detail_Totalmoney = BeginPrice;
                    }
                    Session["DetailOrder"] = NewOrderDetail;
                    return RedirectToAction("Step3");
                }
                else
                {
                    NewOrderDetail.detail_PayName = orderDetailValid.detail_ShipName;
                    NewOrderDetail.detail_PayLastName = orderDetailValid.detail_ShipLastName;
                    NewOrderDetail.detail_PayPhone = orderDetailValid.detail_ShipPhone;
                    NewOrderDetail.detail_PayEmail = orderDetailValid.detail_ShipEmail;
                    NewOrderDetail.detail_PayProvince = orderDetailValid.detail_ShipProvince;
                    NewOrderDetail.detail_PayDistrict = orderDetailValid.detail_ShipDistrict;
                    NewOrderDetail.detail_PayTown = orderDetailValid.detail_ShipTown;
                    NewOrderDetail.detail_PayStreet = orderDetailValid.detail_ShipStreet;
                    NewOrderDetail.detail_ShipLastName = orderDetailValid.detail_ShipLastName;
                    NewOrderDetail.detail_ShipName = orderDetailValid.detail_ShipName;
                    NewOrderDetail.detail_ShipEmail = orderDetailValid.detail_ShipEmail;
                    NewOrderDetail.detail_ShipPhone = orderDetailValid.detail_ShipPhone;
                    NewOrderDetail.detail_ShipProvince = orderDetailValid.detail_ShipProvince;
                    NewOrderDetail.detail_ShipDistrict = orderDetailValid.detail_ShipDistrict;
                    NewOrderDetail.detail_ShipTown = orderDetailValid.detail_ShipTown;
                    NewOrderDetail.detail_ShipStreet = orderDetailValid.detail_ShipStreet;
                    if (user != null)
                    {
                        var itemview = db.Orders.Where(x => x.user_ID == user.user_ID && x.paid == false).ToList();
                        foreach (var item in itemview)
                        {
                            BeginPrice = Convert.ToDecimal(BeginPrice + item.cart_TotalPrice);
                        }
                        NewOrderDetail.user_ID = user.user_ID;
                        NewOrderDetail.detail_TotalBegin = BeginPrice;
                        NewOrderDetail.detail_Totalmoney = BeginPrice;
                        Session["DetailOrder"] = NewOrderDetail;
                        return RedirectToAction("Step3");
                    }
                    else
                    {
                        var cartAnonymous = Session[CarstDetailSesstion] as List<Order>;
                        foreach (var item in cartAnonymous)
                        {
                            BeginPrice = Convert.ToDecimal(BeginPrice + item.cart_TotalPrice);
                        }
                        NewOrderDetail.detail_TotalBegin = BeginPrice;
                        NewOrderDetail.detail_Totalmoney = BeginPrice;
                    }
                    Session["DetailOrder"] = NewOrderDetail;
                    return RedirectToAction("Step3");
                }
            }
            catch   
            {
                var errormessage = "Vui lòng nhập đầy đủ thông tin";
                ModelState.AddModelError("error", errormessage);
                var cartAnonymous = Session[CarstDetailSesstion] as List<Order>;
                ViewBag.cartano = cartAnonymous;
                return View("Step2");
            }
        }
        public ActionResult Step3()
        {

            try
            {
                var user = Session["User"] as User;
                ViewBag.OrderDetail = Session["DetailOrder"] as OrderDetail;
                ViewBag.datauser = user;
                if (user != null)
                {
                    var itemview = db.Orders.Where(x => x.user_ID == user.user_ID && x.paid == false).ToList();
                    ViewBag.cart = itemview;
                    return View(ViewBag.cart);
                }
                else
                {
                    var cartAnonymous = Session[CarstDetailSesstion] as List<Order>;
                    ViewBag.cartano = cartAnonymous;
                    return View(ViewBag.cartano);
                }
            }
            catch
            {
                return RedirectToAction("Step2");
            }
        }
        [HttpPost]
        public ActionResult Step3(string value, string note)
        {
            var user = Session["User"] as User;
            var OrderDetail = Session["DetailOrder"] as OrderDetail;
            var notify = new Notify();
            int id;
            string paymethod = null;
            if(value == "1")
            {
                paymethod = "Thanh toán COD";
            }
            else
            {
                paymethod = "Thanh toán ZaloPay";
            }
            if (user != null)
            {
                var itemview = db.Orders.Where(x => x.user_ID == user.user_ID && x.paid == false).ToList();
                OrderDetail.detail_dateorder = DateTime.Now;
                OrderDetail.detail_note = note;
                OrderDetail.Paymethod = paymethod;
                db.OrderDetails.Add(OrderDetail);
                db.SaveChanges();
                id = OrderDetail.detail_ID;
                foreach (var item in itemview)
                {
                    item.detail_ID = id;
                    item.paid = true;
                }
                notify.ID = id;
                notify.status = true;
                db.Notifies.Add(notify);                
            }
            else
            {
                var cartAnonymous = Session[CarstDetailSesstion] as List<Order>;
                OrderDetail.detail_dateorder = DateTime.Now;
                OrderDetail.detail_note = note;
                OrderDetail.Paymethod = paymethod;
                db.OrderDetails.Add(OrderDetail);
                db.SaveChanges();
                id = OrderDetail.detail_ID;
                foreach (var item in cartAnonymous)
                {
                    item.detail_ID = id;
                    item.paid = true;
                    db.Orders.Add(item);
                    Session[CarstDetailSesstion] = cartAnonymous;
                }
                db.SaveChanges();
                notify.ID = id;
                notify.status = true;
                db.Notifies.Add(notify);
            }
            db.SaveChanges();
            Session["IDpayment"] = id;
            if (value == "1")
            {
                Session.Contents.Remove(CartSesstion);
                Session.Contents.Remove(CarstDetailSesstion);
                Session.Contents.Remove("DetailOrder");
                return RedirectToAction("Complete", "Payment");
            }
            else
            {
                return RedirectToAction("Checkout", "Payment");

            }
        }
        public ActionResult Complete()
        {
            //var id = Convert.ToInt64(ViewData["IDpayment"]);

            //ViewBag.InforPay = db.OrderDetails.SingleOrDefault(x=>x.detail_ID == id);

            return View();
        }
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        //Đăng nhập bằng FB
        [AllowAnonymous]
        public ActionResult LoginFaceBook()
        {

            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                reponse_type = "code",
                scope = "email"
            });
            return Redirect(loginUrl.AbsoluteUri);
        }

        //Đăng nhập bằng FB
        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });
            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string userName = me.email;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;
                var UsersExit = db.Users.SingleOrDefault(x => x.user_Email == email);
                try
                {
                    if (UsersExit == null)
                    {
                        var user = new User();
                        user.role_ID = 3;
                        user.user_Email = email;
                        user.user_Phone = "";
                        user.user_Name = firstname;
                        user.user_LastName = middlename + lastname;
                        user.user_PassWord = "";
                        user.Accept = false;
                        db.Users.Add(user);
                        Session["User"] = user;
                        db.SaveChanges();
                        return RedirectToAction("Step2", "Payment");
                    }
                    else
                    {
                        Session["User"] = UsersExit;
                        return RedirectToAction("Step2", "Payment");
                    }
                }
                catch (System.IndexOutOfRangeException ex)
                {
                    System.ArgumentException argEx = new System.ArgumentException("Đã có lỗi sảy ra trên hệ thống");
                    throw argEx;
                }
            }
            else
            {
                return RedirectToAction("Badrequest", "Support");
            }
        }

        //Chèn dữ liệu vào FB
        //public int InserForFB(User AI)
        //{
        //    var user_db = db.Users.SingleOrDefault(x => x.user_Loginname == AI.user_Loginname);
        //    if (user_db == null)
        //    {
        //        db.Users.Add(AI);
        //        db.SaveChanges();
        //        return AI.user_ID;
        //    }
        //    else
        //    {
        //        return user_db.user_ID;
        //    }
        //}

        //Logout
        [HttpPost]
        public ActionResult GoogleLogin(string email, string name, string gender, string lastname, string location)
        {
            string Email = email;
            string Name = name;
            string LastName = lastname;
            var UsersExit = db.Users.SingleOrDefault(x => x.user_Email == email);
            try
            {
                if (UsersExit == null)
                {
                    var user = new User();
                    user.role_ID = 3;
                    user.user_Email = email;
                    user.user_Phone = "";
                    user.user_Name = Name;
                    user.user_LastName = lastname;
                    user.Accept = false;
                    db.Users.Add(user);
                    Session["User"] = user;
                    db.SaveChanges();
                    return RedirectToAction("Step2", "Payment");
                }
                else
                {
                    Session["User"] = UsersExit;
                    return RedirectToAction("Step2", "Payment");
                }
            }
            catch (System.IndexOutOfRangeException ex)
            {

                System.ArgumentException argEx = new System.ArgumentException("Đã có lỗi sảy ra trên hệ thống");
                throw argEx;
            }
            finally
            {
                RedirectToAction("Step2", "Payment");
            }
        }
        public ActionResult Checkout()
        {
            var PayID = Convert.ToInt32(Session["IDpayment"]);
            if (PayID != 0)
            {
                ViewBag.amout = db.OrderDetails.SingleOrDefault(x => x.detail_ID == PayID);
                return View();
            }
            else
            {
                return RedirectToAction("BadRequest", "Support");
            }
                
        }

        public ActionResult CheckoutQR()
        {
            var PayID = Convert.ToInt32(Session["IDpayment"]);
            if (PayID != 0)
            {
                ViewBag.amout = db.OrderDetails.SingleOrDefault(x => x.detail_ID == PayID);
                return View();
            }
            else
            {
                return RedirectToAction("BadRequest", "Support");
            }
        }
        public ActionResult PaymentResult()
        {
            return View();
        }
        public ActionResult Change()
        {
            var PayID = Convert.ToInt32(Session["IDpayment"]);
            if(PayID == 0)
            {
                return RedirectToAction("BadRequest", "Support");
            }
            string sql = "UPDATE OrderDetail " +
                            "SET Paymethod = 'Thanh toán COD' WHERE detail_ID =" + PayID;
            using (DB_UMAEntities db = new DB_UMAEntities())
            {
                var update = db.Database.ExecuteSqlCommand(sql);
                db.SaveChanges();
                Session.Contents.Remove(CartSesstion);
                Session.Contents.Remove(CarstDetailSesstion);
                Session.Contents.Remove("DetailOrder");
            }
            return RedirectToAction("Complete","Payment");
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult ReceiveCallback()
        {
            return View();
        }
    }
}