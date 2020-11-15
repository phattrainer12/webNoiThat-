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
using HTML_UMA.Models.RoleUser;

namespace HTML_UMA.Controllers
{

    public class AccountController : Controller
    {
        public string CarstDetailSesstion = "CarstDetail";
        public string CartSesstion = "CarstSesstion";
        private DB_UMAEntities db = new DB_UMAEntities();
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        //Xác nhận tài khoản login
        [HttpPost]
        public ActionResult Login(string Email, string PassWord)
        {
            var Cartproduct = Session[CarstDetailSesstion] as List<Order>;
            if (Email != null && PassWord != null)
            {
                var user = db.Users.SingleOrDefault(x => x.user_Email == Email && x.user_PassWord == PassWord && x.type_register == 1);
                if(user != null)
                {
                    if(Cartproduct != null)
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
                        return Redirect("/Admin/QuanLiSanPham/Index");
                    }
                    else
                    {
                        if(user.role_ID == 2)
                        {
                            Session["User"] = user;
                            return Redirect("/Admin/QuanLiSanPham/Index");
                        }
                        else
                        {
                            Session["User"] = user;
                            return RedirectToAction("Index","Product");
                        }
                    }
                }
                else
                {
                    var errormessage = "Tài khoản đăng nhập hoặc mật khẩu sai!";
                    ModelState.AddModelError("error", errormessage);
                    return View();
                }
            }
            return View();
        }
        // Hủy session của user
        public ActionResult Logout()
        {
            Session.Contents.Remove("User");
            return RedirectToAction("Index","Product");
        }
        public ActionResult Signin()
        {
            return View();
        }
        //Đăng kí tài khoản khoản theo type_register = 1
        [HttpPost]
        public ActionResult Signin(Signin sigin)
        {
            try
            {
                if (sigin != null)
                {
                    var UsersExit = db.Users.SingleOrDefault(x => x.user_Email == sigin.Email);
                    if (UsersExit == null)
                    {
                        var user = new User();
                        user.role_ID = 3;
                        user.user_Email = sigin.Email;
                        user.user_Phone = sigin.Phone;
                        user.user_Name = sigin.Name;
                        user.user_LastName = sigin.LastName;
                        user.user_PassWord = sigin.PassWord;
                        user.type_register = 1;
                        user.Accept = sigin.Accept;
                        db.Users.Add(user);
                        db.SaveChanges();
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        var errormessage = "Địa chỉ email đã tồn tại, vui lòng đăng kí bằng email khác! ";                       
                        ModelState.AddModelError("error", errormessage);
                        return View(sigin);
                    }
                    
                }
                else
                {
                    string errormessage = "Lỗi hệ thống, Vui lòng báo cho người quản lí nếu gặp vấn đề này! ";
                    ModelState.AddModelError("error", errormessage);
                        return View(sigin);
                }
            }
            catch (System.IndexOutOfRangeException ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("Đã có lỗi sảy ra trên hệ thống");
                throw argEx;
            }
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
        //get information user from Faacboo API
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
                            user.token = fb.AccessToken;
                            user.user_Email = email;
                            user.user_Phone = "";
                            user.user_Name = firstname;
                            user.user_LastName = middlename + lastname;
                            user.user_PassWord = "";
                            user.type_register = 2;
                            user.Accept = false;
                            db.Users.Add(user);
                            Session["User"] = user;
                            db.SaveChanges();
                        return RedirectToAction("Index", "Product");
                    }
                    else
                    {
                        Session["User"] = UsersExit;
                        return RedirectToAction("Index", "Product");
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
        public ActionResult LogoutFace()
        {
            FormsAuthentication.SignOut();
            Session["user_customer"] = null;
            Session["name_customer"] = null;
            return RedirectToAction("Index", "Home");
        }
        //Google account register
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
                    user.type_register = 3;
                    user.Accept = false;
                    db.Users.Add(user);
                    Session["User"] = user;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    Session["User"] = UsersExit;
                    return RedirectToAction("Index", "Product");
                }
            }
            catch (System.IndexOutOfRangeException ex)
            {
                
                System.ArgumentException argEx = new System.ArgumentException("Đã có lỗi sảy ra trên hệ thống");
                throw argEx;
            }
            finally
            {
                RedirectToAction("Index", "Product");
            }
        }
        //Trang chủ thông tin tài khoản
        public ActionResult UserInformation()
        {
            return View();
        } 
        //Post chỉnh sửa thông tin tài khoản
        [HttpPost]
        [ValidRole(ValidRole = "customer")]
        public ActionResult EditUser(string user_Name, string user_LastName, string user_Phone)
        {
            var user = Session["User"] as User;
            try
            {
                    var changeuser = db.Users.SingleOrDefault(x => x.user_ID == user.user_ID);
                    changeuser.user_Name = user_Name;
                    changeuser.user_LastName = user_LastName;
                    changeuser.user_Phone = user_Phone;
                    db.SaveChanges();

                Session["User"] = db.Users.SingleOrDefault(x => x.user_ID == user.user_ID);
                return RedirectToAction("UserInformation", "Account");
            }
            catch
            {
                string errormessage = "Thông tin không hợp lệ";
                ModelState.AddModelError("error", errormessage);
                return RedirectToAction("UserInformation", "Account");
            }
        }
        //Chỉnh sửa thông tin tài khoản
        public ActionResult EditUser()
        {
            var user = Session["User"] as User;
            if(user == null)
            {
                return RedirectToAction("Login", "Account");
            }else
            {
                return View();
            }           
            
        }
        //Thay đổi mật khẩu tài khoản
        public ActionResult changePassword()
        {
            return View();
        }
        [HttpPost]
        [ValidRole(ValidRole = "customer")]
        public ActionResult changePassword(string oldPass, string newPass)
        {
            var user = Session["User"] as User;
            try
            {
                if(user.user_PassWord == oldPass && newPass.Length >= 6)
                {
                    var userchange = db.Users.SingleOrDefault(x => x.user_ID == user.user_ID);
                    userchange.user_PassWord = newPass;
                    db.SaveChanges();
                    return RedirectToAction("", "");
                }
                else
                {
                    string errormessage = "Mật khẩu hiện tại không đúng! ";
                    ModelState.AddModelError("error", errormessage);
                    return RedirectToAction("", "");
                }
            }
            catch
            {
                string errormessage = "Lỗi hệ thống, Vui lòng báo cho Adminstrator nếu gặp vấn đề này! ";
                ModelState.AddModelError("error", errormessage);
            }
            return View();
        }
        public ActionResult yourPayment(int detail_ID)
        {
            ViewBag.detail = db.OrderDetails.SingleOrDefault(x => x.detail_ID == detail_ID);
            ViewBag.listorder = db.Orders.Where(x => x.detail_ID == detail_ID && x.paid == true).ToList();
            return View();
        }
        [ValidRole(ValidRole = "customer")]
        public ActionResult listFavourist()
        {
            var user = Session["User"] as User;
            var listFav = db.Favourites.Where(x => x.user_ID == user.user_ID).ToList();
            return View(listFav);
        }
        [ValidRole(ValidRole = "customer")]
        public ActionResult Order()
        {
            var user = Session["User"] as User;
            var yourorder = db.OrderDetails.Where(x => x.user_ID == user.user_ID).ToList();
            return View(yourorder);
        }

        public ActionResult getCart()
        {
            var user = Session["User"] as User;
            if (user != null)
            {
                var itemview = db.Orders.Where(x => x.user_ID == user.user_ID).ToList();
                ViewBag.cart = itemview;
                return View();
            }
            else
            {
                var cartAnonymous = Session[CarstDetailSesstion] as List<Order>;
                ViewBag.cart = cartAnonymous;
                return View();
            }
        }

        public ActionResult GetSearchValue(string search)
        {
            var item = db.Products.Where(x => x.pro_Name.Contains(search) | x.pro_Description.Contains(search) | x.pro_code.Contains(search) | x.Color.NameColor.Contains(search)).ToList();
            ViewBag.data = item;
            return View();
        }
    }
}