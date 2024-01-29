using Microsoft.AspNetCore.Mvc;
using OgainShop.Models;
using OgainShop.Data;
using Microsoft.AspNetCore.Http;
using OgainShop.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System;
using System.Text;
using OgainShop.Heplers;

namespace OgainShop.Controllers
{
    public class PageController : Controller
    {
        private readonly OgainShopContext db;

        public PageController(OgainShopContext context)
        {
            db = context;
        }
        [Authentication]
        public async Task<IActionResult> Home()
        {
            // Retrieve distinct categories from the database
            var distinctCategories = await db.Category.ToListAsync();

            // Pass the distinct categories to the view
            ViewBag.Categories = distinctCategories;

            // Retrieve only the top 8 products with their categories
            var productsWithCategories = await db.Product.Include(p => p.Category)
                                                        .Take(8) // Take only 8 products
                                                        .ToListAsync();

            // Pass the products to the view
            ViewBag.Products = productsWithCategories;

            return View();
        }

        [Authentication]
        public async Task<IActionResult> Details(int id)
        {

       
            var cartItems = HttpContext.Session.Get<List<CartItem>>("cart");
            ViewData["CartItemCount"] = cartItems != null ? cartItems.Count : 0;
           
            // Retrieve the product details from the database based on the provided ID
            var product = await db.Product.Include(p => p.Category)
                                          .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                // Handle the case where the product with the specified ID is not found
                return NotFound();
            }

            // Pass the product data to the view
            ViewBag.ProductId = product.ProductId;
            ViewBag.ProductThumbnail = product.Thumbnail;  // Đường dẫn ảnh sản phẩm
            ViewBag.ProductName = product.ProductName; // Tên sản phẩm
            ViewBag.ProductPrice = product.Price;      // Giá sản phẩm
            ViewBag.ProductDescription = product.Description; // Mô tả sản phẩm
            ViewBag.ProductCategory = product.Category?.CategoryName;


            return View(product);
        }


        [Authentication]
        public IActionResult Cart()
        {
            return View();
        }
        [Authentication]
        public IActionResult Checkout()
        {
            return View();
        }
        [Authentication]
        public IActionResult Contact()
        {
            return View();
        }
        [Authentication]
        public IActionResult Blog()
        {
            return View();
        }
        [Authentication]
        public IActionResult Favourite()
        {
            return View();
        }
        [Authentication]
        public async Task<IActionResult> Category()
        {
            var productList = await db.Product.ToListAsync();

            return View(productList);
        }


        [Authentication]
        public IActionResult Thankyou()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Home", "Page");
            }
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                User u = db.User.FirstOrDefault(
                    x => x.Email.Equals(user.Email)); 

                if (u != null && BCrypt.Net.BCrypt.Verify(user.Password, u.Password)) 
                {
                    HttpContext.Session.SetString("Username", u.Username.ToString());
                    HttpContext.Session.SetString("Role", u.Role);

                    if (u.Role == "Admin")
                    {
                        return RedirectToAction("Home", "Page");
                    }
                    else if (u.Role == "User")
                    {
                        return RedirectToAction("Home", "Page");
                    }
                }
            }

            return View();
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (true)
            {
                // Kiểm tra xem tên đăng nhập đã tồn tại chưa
                if (db.User.Any(x => x.Username.Equals(user.Username)))
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại");
                    return View(user);
                }
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                // Thiết lập vai trò mặc định là "User"
                user.Role = "User";

                // Thêm người dùng mới vào cơ sở dữ liệu
                db.User.Add(user);
                db.SaveChanges();

                // Lưu thông tin đăng nhập vào phiên làm việc
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);

                return RedirectToAction("Home", "Page");
            }

            return View(user);
        }



        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Username");
            return RedirectToAction("login", "Page");
        }

       
    }
}