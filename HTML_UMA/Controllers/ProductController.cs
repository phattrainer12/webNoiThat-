using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTML_UMA.Models;
using HTML_UMA.ModelConfirm;
using Newtonsoft.Json;

namespace HTML_UMA.Controllers
{
    public class ProductController : Controller
    {
        public string CarstDetailSesstion = "CarstDetail";
        public string CartSesstion = "CarstSesstion";
        private DB_UMAEntities db = new DB_UMAEntities();
        // GET: Product
        public ActionResult Menu()
        {
            return View();
        }
        public ActionResult Index()
        {
            var product = db.Products.OrderByDescending(x=>x.pro_Date).Take(80).ToList();
            return View(product);
        }
        // them san pham vao gio hang
        public ActionResult addcart(int pro_ID)
        {
            //session user
            var user = Session["User"] as User;
            //create list order to save shopping cart!     
            var item = db.Products.SingleOrDefault(x => x.pro_ID == pro_ID);
            var addcart = new Order();
            addcart.pro_ID = item.pro_ID;
            addcart.pro_Name = item.pro_Name;
            addcart.cart_Quanty = 1;
            addcart.pro_Price = item.pro_price;
            addcart.pro_Image = item.pro_Image;
            addcart.pro_Instruction = item.pro_Instruction;
            addcart.pro_Materials = item.pro_Materials;
            addcart.pro_Size = item.pro_Size;
            addcart.pro_Source = item.pro_Source;
            addcart.pro_View = item.pro_View;
            addcart.Color_ID = item.Color_ID;
            addcart.cart_TotalPrice = Convert.ToDecimal(addcart.cart_Quanty * item.pro_price);
            addcart.paid = false;
            try
            {

                if (Session["User"] == null)
                {
                    //Kiểm tra trong giỏ hàng (SS) có null hay không nếu null sẽ thêm mới mặt hàng này vào trong Cart(SS) 
                    if (Session[CarstDetailSesstion] == null)
                    {
                        List<Order> ListCartSS = new List<Order>();
                        ListCartSS.Add(addcart);
                        Session[CarstDetailSesstion] = ListCartSS;
                        var count = ListCartSS.Count();
                        return Json(count);
                    }
                    else //Trong trường hợp giỏ hàng đã tồn tại các mặt hàng
                    {
                        var ListCartSS = (List<Order>)Session[CarstDetailSesstion]; // Lấy Ds những mặt hàng nằm trong Cart(SS)
                        if (ListCartSS.Exists(x => x.pro_ID == pro_ID))// Trong trường hợp mà hàng mà khách hàng click đã tồn tại trong giỏ hàng thì + số lượng mặt hàng đó lên 1
                        {
                            foreach (var x in ListCartSS)
                            {
                                if (x.pro_ID == pro_ID)
                                {
                                    var QuantityProduct = x.cart_Quanty + 1;
                                    // Vì giới hạn mỗi sản phẩm chỉ mua tối đa số lượng là 4 nên khi + vào trong DB cũng cần phải kiêm tra nó có vượt qua 4 sản phẩm hay không/
                                    if (QuantityProduct > 4)
                                    {
                                        return Json("Sản phẩm đã vượt quá giới hạn mua 5 sản phẩm");
                                    }
                                    else //nếu không vượt quá thì + bình thường.
                                    {
                                        x.cart_Quanty += 1;
                                        x.cart_TotalPrice = Convert.ToDecimal(x.cart_Quanty * (int)x.pro_Price);
                                    }
                                }
                            }
                            Session[CarstDetailSesstion] = ListCartSS;
                            var count = ListCartSS.Count();
                            return Json(count);
                        }
                        else // Nếu trong  ss không có mặt hàng đó thì thêm mới vào Cart (SS)
                        {
                            ListCartSS.Add(addcart);
                            Session[CarstDetailSesstion] = ListCartSS;
                            var count = ListCartSS.Count();
                            return Json(count);
                        }
                        
                    }
                }
                else /// Nếu trường hợp lúc này khách hàng đã đăng nhập thì cập nhật luôn vào Database
                {
                    if (Session[CarstDetailSesstion] == null)
                    {
                        var exititem = db.Orders.Where(x => x.user_ID == user.user_ID).ToList();
                        if (exititem.Exists(x => x.pro_ID == pro_ID))
                        {
                            foreach (var x in exititem)
                            {
                                if (x.pro_ID == pro_ID)
                                {
                                    var QuantityProduct = x.cart_Quanty + 1;
                                    // Vì giới hạn mỗi sản phẩm chỉ mua tối đa số lượng là 4 nên khi + vào trong DB cũng cần phải kiêm tra nó có vượt qua 4 sản phẩm hay không/
                                    if (QuantityProduct > 4)
                                    {
                                        return Json("Sản phẩm đã vượt quá giới hạn mua 5 sản phẩm");
                                    }
                                    else //nếu không vượt quá thì + bình thường.
                                    {   
                                        x.cart_Quanty += 1;
                                        x.cart_TotalPrice = Convert.ToDecimal(x.cart_Quanty * x.pro_Price);
                                    }
                                }
                            }
                        }
                        else
                        {
                            addcart.user_ID = user.user_ID;
                            db.Orders.Add(addcart);
                        }
                        db.SaveChanges();
                        var count = exititem.Count();
                        return Json(count);
                    }
                    else
                    {
                        var itemcart = Session[CarstDetailSesstion] as List<Order>;
                        foreach (var x in itemcart)
                        {
                            x.user_ID = user.user_ID;
                            db.Orders.Add(x);
                        }
                        Session.Contents.Remove(CarstDetailSesstion);                   
                    }
                    return Json("");
                }
            }
            catch
            {
                return Json("Có lỗi sảy ra");
            }
        }
        public ActionResult AddFavourist(int pro_ID)
        {
            var user = Session["User"] as User;
            Favourite newfavourist = new Favourite();
            var itemfavourist = db.Products.SingleOrDefault(x => x.pro_ID == pro_ID);
            var exitsitem = db.Favourites.SingleOrDefault(x => x.pro_ID == pro_ID);
            try
            {
                if (exitsitem == null)
                {
                    newfavourist.pro_ID = itemfavourist.pro_ID;
                    newfavourist.user_ID = user.user_ID;
                    db.Favourites.Add(newfavourist);
                    db.SaveChanges();
                }
                return Json("addWishlist");
            }
            catch
            {
                return Json("false");
            }
        }
        public ActionResult getFavourist()
        {
            var user = Session["User"] as User;
            if(user != null){
                var itemFav = db.Favourites.Where(x => x.user_ID == user.user_ID).ToList();
                ViewBag.favourist = itemFav;
                return View();
            }
            else
            {
                ViewBag.Message = "bạn cần đăng nhập để mua hàng";
                return View();
            }
            
        }
        public ActionResult CategoryProduct(int Menu_ID)
        {
            var item = db.Products.Where(x => x.Menu_ID == Menu_ID | x.Menu.ParentIid == Menu_ID).ToList();
            ViewBag.NameCategory = db.Menus.SingleOrDefault(x => x.Menu_ID == Menu_ID);
            ViewBag.Categorychild = db.Menus.Where(x => x.ParentIid == Menu_ID).ToList();
            return View(item);           
        }
        public ActionResult RoomProduct(int Menu_ID)
        {
            var item = db.Products.Where(x => x.Menu_ID == Menu_ID).ToList();
            ViewBag.NameCategory = db.Menus.SingleOrDefault(x => x.Menu_ID == Menu_ID);
            ViewBag.Categorychild = db.Menus.Where(x => x.ParentIid == Menu_ID).ToList();
            return View(item);
        }
        public ActionResult GroupProduct(int group_ID)
        {
            var item = db.Products.Where(x => x.group_ID == group_ID).ToList();
            ViewBag.NameGroup = db.GroupProducts.SingleOrDefault(x => x.group_ID == group_ID);
            return View(item);
        }
        //Trang san pham moi

    
        // tim kiem san pham theo gia
        public ActionResult Price(decimal beginprice, decimal endprice, int Menu_ID )
        {
            var item = db.Database.SqlQuery<Product>("SELECT * FROM Product WHERE Menu_ID =" + Menu_ID +"  AND pro_price > "+beginprice+ "AND pro_price < " + endprice).ToList();
            return View(item);
        }
        public ActionResult PriceDes(decimal beginprice, decimal endprice)
        {
            var item = db.Products.Where(x => x.pro_price > beginprice && x.pro_price < endprice).ToList();
            return View(item);
        }
        public ActionResult SearchColorDes(int color)
        {
            var item = db.Database.SqlQuery<Product>("SELECT * FROM Product WHERE Color_ID=" + color+ "ORDER BY pro_Date DESC").ToList();
            return View(item);
        }
        public ActionResult PriceHotProduct(decimal beginprice, decimal endprice)
        {
            var item = db.Database.SqlQuery<Product>("SELECT * FROM Product WHERE pro_price >"+ beginprice+" and pro_price < "+ endprice+ " ORDER BY pro_View DESC").ToList();
            return View(item);
        }
        public ActionResult SearchColorHotProduct(int color)
        {
            var item = db.Database.SqlQuery<Product>("SELECT * FROM Product WHERE Color_ID="+ color).ToList();
            return View(item);
        }
        public ActionResult Search(string value)
        {
            var item = db.Products.Where(x => x.pro_Name.Contains(value) | x.pro_Description.Contains(value) | x.pro_code.Contains(value) | x.Color.NameColor.Contains(value)).ToList();
            return View(item);
        }
        //search auto complete
        public ActionResult GetSearchValue(string search)
        {
            var item = db.Products.Where(x => x.pro_Name.Contains(search) | x.pro_Description.Contains(search) | x.pro_code.Contains(search) | x.Color.NameColor.Contains(search)).ToList();
            ViewBag.data = item;
            return View();
        }
            public ActionResult About()
        {
            return View();
        }
        public ActionResult Product()   
        {
            List<Menu> menu = db.Menus.Where(x => x.ParentIid == 1).ToList();
            return View(menu);
        }
        public ActionResult Room()
        {
            List<Menu> room = db.Menus.Where(x => x.ParentIid == 2).ToList();
            return View(room);
        }
        public ActionResult Group()
        {
            List<GroupProduct> group = db.GroupProducts.ToList();
            return View(group);
        }
        public ActionResult ProductDetail(int? pro_ID)
        {
            if (pro_ID == null)
            {
                return RedirectToAction("BadRequest", "Support");
            }
            var item = db.Products.SingleOrDefault(x => x.pro_ID == pro_ID);
            ViewBag.moreImage = db.LinkImages.Where(x => x.pro_ID == pro_ID).ToList();
            return View(item);
        }
        public  ActionResult Cart()
        {
            var user = Session["User"] as User;
            if (user != null)
            {
                var itemview = db.Orders.Where(x => x.user_ID == user.user_ID && x.paid == false).ToList();
                return View(itemview);
            }
            else
            {
                var cartAnonymous = Session[CarstDetailSesstion] as List<Order>;
                return View(cartAnonymous);
            }
        }
        public  ActionResult CheckUser()
        {
            var user = Session["User"] as User;
            if(user != null)
            {
                return RedirectToAction("Step2", "Payment");
            }
            else
            {
                return RedirectToAction("Step1", "Payment");
            }
        }
        public ActionResult ChangeQuanty(int newquanty, int cart_ID, int pro_ID)
        {
            var user = Session["User"] as User;
            var cartanony = Session[CartSesstion] as List<Order>;
            if (user != null)
            {
                Order changeCart = db.Orders.SingleOrDefault(x => x.user_ID == user.user_ID && x.cart_ID == cart_ID);
                if (changeCart.cart_Quanty != newquanty && newquanty <= 6 && newquanty >= 1)
                {
                    changeCart.cart_Quanty = newquanty;
                    changeCart.cart_TotalPrice = newquanty * changeCart.pro_Price;
                }
                db.SaveChanges();
                return RedirectToAction("Cart");
            }
            else
            {
                var ListCartSS = (List<Order>)Session[CarstDetailSesstion]; // Lấy Ds những mặt hàng nằm trong Cart(SS)
                var itemchange = ListCartSS.SingleOrDefault(x => x.pro_ID == pro_ID);
                if (itemchange.cart_Quanty != newquanty && newquanty <= 6 && newquanty >= 1)
                {
                    itemchange.cart_Quanty = newquanty;
                    itemchange.cart_TotalPrice = newquanty * itemchange.pro_Price;
                }
                return RedirectToAction("Cart");
            }
        }
        public ActionResult DeleteItemCart(int pro_ID, int cart_ID)
        {
            var user = Session["User"] as User;
            var cartanony = Session[CartSesstion] as List<Order>;
            if (user != null)
            {
                Order removeCart = db.Orders.SingleOrDefault(x => x.user_ID == user.user_ID && x.cart_ID == cart_ID);
                db.Orders.Remove(removeCart);
                db.SaveChanges();
            }
            else
            {
                var LitsCarrtSS = (List<Order>)Session[CarstDetailSesstion];
                if (LitsCarrtSS.Exists(x => x.pro_ID == pro_ID && x.cart_ID == cart_ID))
                {
                    var itemCart = LitsCarrtSS.Where(x => x.pro_ID == pro_ID && x.cart_ID == cart_ID).SingleOrDefault();
                    LitsCarrtSS.Remove(itemCart);
                }
            }
            return RedirectToAction("Cart");
        }
        public ActionResult Catalogue2017()
        {
            return View();
        }
        public ActionResult getCart()
        {
            var user = Session["User"] as User;
            if (user != null)
            {
                var itemview = db.Orders.Where(x => x.user_ID == user.user_ID && x.paid == false).ToList();
                ViewBag.cart = itemview;
                return View();
            }
            else
            {
                var cartAnonymous = Session[CarstDetailSesstion] as List<Order>;
                if(cartAnonymous != null)
                {
                    ViewBag.cart = cartAnonymous.ToList();
                }
                else
                {
                    ViewBag.cart = null;
                }
                return View();
            }
        }
        public ActionResult InteriorDecoration()
        {
            var interior = db.Menus.Where(x => x.ParentIid == 1).ToList();
            return View(interior);
        }
        public ActionResult Magestore()
        {
            return View();
        }
        public ActionResult GroupIndex()
        {
            var group = db.GroupProducts.ToList();
            return View(group);
        }
        public ActionResult getCountProduct(int Menu_ID)
        {
            ViewBag.Count = db.Products.Where(x => x.Menu_ID == Menu_ID | x.Menu.ParentIid == Menu_ID).Count();
            return View();
        }
        public ActionResult getCountGroup(int Group_ID)
        {
          ViewBag.Count = db.Products.Where(x => x.group_ID == Group_ID).Count();
            return View();
        }
        public string ModelSearch()
        {
            var Value = db.Products.ToList();
            var Res = new List<Search>();
            foreach (var item in Value)
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
    }
}