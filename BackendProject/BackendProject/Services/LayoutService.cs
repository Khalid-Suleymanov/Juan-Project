using BackendProject.DAL;
using BackendProject.Models;
using BackendProject.ViewModels;
using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace BackendProject.Services
{
    public class LayoutService
    {
        private readonly ProjectDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LayoutService(ProjectDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public List<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public List<Brand> GetBrands()
        {
            return _context.Brands.ToList();
        }
        public List<Product> GetProducts()
        {
            return _context.Products.ToList();
        }





        public BasketViewModel GetBasket()
        {
            var basketVM = new BasketViewModel();

            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var databaseItems = _context.BasketItems.Include(x => x.Product).ThenInclude(x => x.Images.Where(bi => bi.ImageStatus == true)).Where(x => x.AppUserId == userId).ToList();
                foreach (var dbItem in databaseItems)
                {
                    BasketItemVM item = new BasketItemVM
                    {
                        Count = dbItem.Count,
                        Product = _context.Products.Include(x => x.Images).FirstOrDefault(x => x.Id == dbItem.ProductId)
                    };
                    basketVM.basketItems.Add(item);
                    basketVM.TotalAmount += (item.Product.DiscountedPrice > 0 ? item.Product.DiscountedPrice : item.Product.SalePrice) * item.Count;
                }
            }
            else
            {
                var basketStr = _httpContextAccessor.HttpContext.Request.Cookies["basket"];

                if (basketStr != null)
                {
                    List<BasketCookieItemViewModel> cookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemViewModel>>(basketStr);

                    foreach (var cookieItem in cookieItems)
                    {
                        BasketItemVM item = new BasketItemVM
                        {
                            Count = cookieItem.Count,
                            Product = _context.Products.Include(x => x.Images).FirstOrDefault(x => x.Id == cookieItem.ProductId)
                        };
                        basketVM.basketItems.Add(item);
                        basketVM.TotalAmount += (item.Product.DiscountedPrice > 0 ? item.Product.DiscountedPrice : item.Product.SalePrice) * item.Count;
                    }
                }
            }

            return basketVM;

        }
    }
}
