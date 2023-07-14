using BackendProject.DAL;
using BackendProject.Models;
using BackendProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace BackendProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProjectDbContext _context;
        public HomeController(ProjectDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Sliders = _context.Sliders.OrderBy(x => x.Order).ToList(),
                Features = _context.Features.Take(3).ToList(),
                FourBrands = _context.FourBrands.Take(5).ToList(),
                SportOffs = _context.SportOffs.Take(2).ToList(),
                Products = _context.Products
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.Color)
                .Include(x => x.ProductSizes)
                .ThenInclude(p => p.Size)
                .Include(x => x.Images.Where(x => x.ImageStatus == true)).Take(5).ToList(),
                NewProducts = _context.Products
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.Color)
                .Include(x => x.ProductSizes)
                .ThenInclude(p => p.Size)
                .Include(x => x.Images.Where(x => x.ImageStatus == true)).Take(6).ToList(),
            };
            return View(homeVM);
        }




        public IActionResult GetDetail(int id)
        {
            Product product = _context.Products.Include(x => x.Images).FirstOrDefault(x => x.Id == id);
            return PartialView("_ProductModalPartial", product);
        }



        public IActionResult AddToBasket(int id)
        {

            BasketViewModel basketVM = new BasketViewModel();

            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var basketItems = _context.BasketItems.Where(x => x.AppUserId == userId).ToList();

                var basketItem = basketItems.FirstOrDefault(x => x.ProductId == id);

                if (basketItem == null)
                {
                    basketItem = new BasketItem
                    {
                        ProductId = id,
                        Count = 1,
                        AppUserId = userId,
                    };
                    _context.BasketItems.Add(basketItem);
                }
                else
                {
                    basketItem.Count++;
                }
                _context.SaveChanges();
                var items = _context.BasketItems.Include(x => x.Product).ThenInclude(x => x.Images.Where(pi => pi.ImageStatus == true)).Where(x => x.AppUserId == userId).ToList();

                foreach (var pi in items)
                {
                    BasketItemVM item = new BasketItemVM
                    {
                        Count = pi.Count,
                        Product = pi.Product,
                    };
                    basketVM.basketItems.Add(item);
                    basketVM.TotalAmount += (item.Product.DiscountedPrice > 0 ? item.Product.DiscountedPrice : item.Product.SalePrice) * item.Count;
                }
            }
            else
            {
                var basketStr = Request.Cookies["basket"];
                List<BasketCookieItemViewModel> cookieItems = null;
                if (basketStr == null)
                {
                    cookieItems = new List<BasketCookieItemViewModel>();
                }
                else
                {
                    cookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemViewModel>>(basketStr);
                }
                BasketCookieItemViewModel cookieItem = cookieItems.FirstOrDefault(x => x.ProductId == id);
                if (cookieItem == null)
                {
                    cookieItem = new BasketCookieItemViewModel
                    {
                        ProductId = id,
                        Count = 1
                    };
                    cookieItems.Add(cookieItem);
                }
                else
                {
                    cookieItem.Count++;
                }
                HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(cookieItems));
                foreach (var ci in cookieItems)
                {
                    BasketItemVM item = new BasketItemVM
                    {
                        Count = ci.Count,
                        Product = _context.Products.Include(x => x.Images.Where(x => x.ImageStatus == true)).FirstOrDefault(x => x.Id == ci.ProductId)
                    };
                    basketVM.basketItems.Add(item);
                    basketVM.TotalAmount += (item.Product.DiscountedPrice > 0 ? item.Product.DiscountedPrice : item.Product.SalePrice) * item.Count;
                }
            }

            return PartialView("_BasketPartial", basketVM);
        }


          
        public IActionResult ShowBasket()
        {
            var datastr = HttpContext.Request.Cookies["basket"];
            var data = JsonConvert.DeserializeObject<List<BasketCookieItemViewModel>>(datastr);
            return Json(data);
        }



        public IActionResult GetBasketCount()
        {
            string? dataStr = HttpContext.Request.Cookies["basket"];
            var data = dataStr == null ? null : JsonConvert.DeserializeObject<List<BasketCookieItemViewModel>>(dataStr);
            var count = data?.Count ?? 0;
            var response = new
            {
                count
            };
            return Json(response);
        }
        public IActionResult RemoveFromBasket(int id)
        {
            var basketStr = Request.Cookies["basket"];

            List<BasketCookieItemViewModel> cookieItems = null;

            if (basketStr == null)
            {
                cookieItems = new List<BasketCookieItemViewModel>();
            }
            else
            {
                cookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemViewModel>>(basketStr);
            }
            var itemToRemove = cookieItems.FirstOrDefault(x => x.ProductId == id);
            if (itemToRemove != null)
            {
                cookieItems.Remove(itemToRemove);
                HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(cookieItems));
            }
            BasketViewModel basketVM = new BasketViewModel();
            if (cookieItems != null)
            {
                foreach (var ci in cookieItems)
                {
                    BasketItemVM item = new BasketItemVM
                    {
                        Count = ci.Count,
                        Product = _context.Products.Include(x => x.Images.Where(x => x.ImageStatus == true)).FirstOrDefault(x => x.Id == ci.ProductId)
                    };
                    basketVM.basketItems.Add(item);
                    basketVM.TotalAmount += (item.Product.DiscountedPrice > 0 ? item.Product.DiscountedPrice : item.Product.SalePrice) * item.Count;
                }
            }
            return PartialView("_BasketPartial", basketVM);
        }
    }

}