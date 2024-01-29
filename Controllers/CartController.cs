using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OgainShop.Data;
using OgainShop.Models;
using Microsoft.AspNetCore.Session;
using OgainShop.Heplers;
using Newtonsoft.Json;
using System.Text;

namespace OgainShop.Controllers
{
    public class CartController : Controller
    {

        private readonly OgainShopContext _context;

        public CartController(OgainShopContext context)
        {
            _context = context;
        }


        public List<CartItem> Carts
        {
            get
            {
                //var data = HttpContext.Session.Get<List<CartItem>>("cart");
                var data = HttpContext.Session.Get<List<CartItem>>("cart");
                if (data != null)
                {
                    data = new List<CartItem>();
                }
                return data;
            }
        }
        //public IActionResult AddToCart(int id)
        //{
        //    var myCart = Carts;
        //    var item = myCart.SingleOrDefault(p => p.ProductId == id);
        //    if (item == null)
        //    {
        //        var product = _context.Product.SingleOrDefault(p => p.ProductId == id);
        //        item = new CartItem
        //        {
        //            ProductId = id,
        //            ProductName = product.ProductName,
        //            Qty = product.Qty,
        //            Thumbnail = product.Thumbnail,
        //            Price = product.Price
        //        };
        //    }
        //    else
        //    {
        //        item.Qty++;
        //    }
        //    HttpContext.Session.Set("cart", myCart);
        //    return RedirectToAction("Index");
        //}
        public IActionResult AddToCart(int id)
        {

            List<CartItem> myCart;

            // Kiểm tra nếu Session chứa giỏ hàng
            if (HttpContext.Session.TryGetValue("cart", out byte[] cartBytes))
            {
                myCart = JsonConvert.DeserializeObject<List<CartItem>>(Encoding.UTF8.GetString(cartBytes));
            }
            else
            {
                // Nếu không, tạo một giỏ hàng mới
                myCart = new List<CartItem>();
            }

            // Tìm sản phẩm trong giỏ hàng
            var item = myCart.SingleOrDefault(p => p.ProductId == id);

            if (item == null)
            {
                // Nếu sản phẩm không có trong giỏ hàng, thêm sản phẩm mới vào giỏ hàng
                var product = _context.Product.SingleOrDefault(p => p.ProductId == id);

                if (product != null)
                {
                    myCart.Add(new CartItem
                    {
                        ProductId = id,
                        ProductName = product.ProductName,
                        Qty = 1, // Bạn có thể cần cập nhật số lượng một cách thích hợp
                        Thumbnail = product.Thumbnail,
                        Price = product.Price
                    });
                }
            }
            else
            {
                // Nếu sản phẩm đã có trong giỏ hàng, tăng số lượng lên
                item.Qty++;
            }

            // Cập nhật giỏ hàng trong Session
            HttpContext.Session.Set("cart", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(myCart)));

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int id)
        {
            List<CartItem> myCart;

            if (HttpContext.Session.TryGetValue("cart", out byte[] cartBytes))
            {
                myCart = JsonConvert.DeserializeObject<List<CartItem>>(Encoding.UTF8.GetString(cartBytes));
            }
            else
            {
                myCart = new List<CartItem>();
            }

            var item = myCart.SingleOrDefault(p => p.ProductId == id);

            if (item != null)
            {
                myCart.Remove(item);
                HttpContext.Session.Set("cart", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(myCart)));
            }

            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            // Lấy danh sách sản phẩm từ Session hoặc từ bất kỳ nguồn dữ liệu nào khác
            List<CartItem> cartItems = HttpContext.Session.Get<List<CartItem>>("cart");

            return View(cartItems);
        }

    }
}
