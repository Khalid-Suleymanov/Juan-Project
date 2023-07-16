using BackendProject.DAL;
using BackendProject.Models;
using BackendProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;

namespace BackendProject.Controllers
{
    public class OrderController : Controller
    {
        private readonly ProjectDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public OrderController(ProjectDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Checkout()
        {
            CheckOutViewModel vm = new CheckOutViewModel();
            vm.Order = new OrderCreateViewModel();
            string userId = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
            vm.Items = _generateCheckoutItem(userId);
            vm.TotalAmount = vm.Items.Sum(x => x.Price);
            if (userId != null)
            {
                AppUser user = _userManager.FindByIdAsync(userId).Result;
                vm.Order.FullName = user.FullName;
                vm.Order.Phone = user.PhoneNumber;
                vm.Order.Email = user.Email;
            }
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateViewModel orderVM)
        {
            string userId = (User.Identity.IsAuthenticated && User.IsInRole("Member")) ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
            AppUser user = (User.Identity.IsAuthenticated && User.IsInRole("Member")) ? await _userManager.FindByIdAsync(userId) : null;
            if (!ModelState.IsValid)
            {
                CheckOutViewModel vm = new CheckOutViewModel();
                vm.Order = orderVM;
                vm.Items = _generateCheckoutItem(userId);
                vm.TotalAmount = vm.Items.Sum(x => x.Price);
                return View("Checkout", vm);
            }
            Order order = new Order
            {
                FullName = user == null ? orderVM.FullName : user.FullName,
                Address = orderVM.Address,
                Email = user == null ? orderVM.Email : user.Email,
                Phone = user == null ? orderVM.Phone : user.PhoneNumber,
                Note = orderVM.Note,
                CreatedAt = DateTime.UtcNow.AddHours(4),
                Status = Enums.OrderStatus.Pending,
                AppUserId = userId,
                OrderItems = _generateOrderItems(userId),
            };
            order.TotalAmount = order.OrderItems.Sum(x => x.Count * (x.DiscountedPrice > 0 ? x.DiscountedPrice : x.UnitSalePrice));
            _context.Orders.Add(order);
            _context.SaveChanges();
            _clearBasket(userId);
            if (userId != null)
            {
                return RedirectToAction("profile", "account", new { tab = "Orders"});
            }
            TempData["Success"] = "Order created successfuly!";
            return RedirectToAction("index", "home");
        }
        private List<OrderItem> _generateOrderItems(string userId = null)
        {
            List<OrderItem> items = new List<OrderItem>();
            if (userId != null)
            {
                var basketItems = _context.BasketItems.Include(x => x.Product).Where(x => x.AppUserId == userId).ToList();
                items = basketItems.Select(x =>
                new OrderItem
                {
                    ProductId = x.ProductId,
                    Count = x.Count,
                    UnitCostPrice = x.Product.CostPrice,
                    UnitSalePrice = x.Product.SalePrice,
                    DiscountedPrice = x.Product.DiscountedPrice,
                }).ToList();
            }
            else
            {
                var basketStr = Request.Cookies["basket"];
                if (basketStr != null)
                {
                    var basketItems = JsonConvert.DeserializeObject<List<BasketCookieItemViewModel>>(basketStr);
                    foreach (var pi in basketItems)
                    {
                        var product = _context.Products.Find(pi.ProductId);
                        OrderItem orderItem = new OrderItem
                        {
                            ProductId = pi.ProductId,
                            Count = pi.Count,
                            UnitSalePrice = product.SalePrice,
                            UnitCostPrice = product.CostPrice,
                            DiscountedPrice = product.DiscountedPrice,
                        };
                        items.Add(orderItem);
                    }
                }
            }
            return items;
        }
        private List<CheckOutItemViewModel> _generateCheckoutItem(string userId = null)
        {
            List<CheckOutItemViewModel> items = new List<CheckOutItemViewModel>();
            if (userId != null)
            {
                var basketItems = _context.BasketItems.Include(x => x.Product).Where(x => x.AppUserId == userId).ToList();
                items = basketItems.Select(x => new CheckOutItemViewModel
                {
                    Count = x.Count,
                    Product = x.Product.Name,
                    Price = x.Count * (x.Product.DiscountedPrice > 0 ? x.Product.DiscountedPrice : x.Product.SalePrice)
                }).ToList();
            }
            else
            {
                string basketStr = Request.Cookies["basket"];
                if (basketStr != null)
                {
                    List<BasketCookieItemViewModel> basketItems = JsonConvert.DeserializeObject<List<BasketCookieItemViewModel>>(basketStr);
                    foreach (var item in basketItems)
                    {
                        var product = _context.Products.Find(item.ProductId);
                        var checkoutItem = new CheckOutItemViewModel
                        {
                            Count = item.Count,
                            Product = product.Name,
                            Price = item.Count * (product.DiscountedPrice > 0 ? product.DiscountedPrice : product.SalePrice)
                        };
                        items.Add(checkoutItem);
                    }
                }
            }
            return items;
        }
        private void _clearBasket(string userId = null)
        {
            if (userId == null)
            {
                Response.Cookies.Delete("basket");
            }
            else
            {
                _context.RemoveRange(_context.BasketItems.Where(x => x.AppUserId == userId).ToList());
                _context.SaveChanges();
            }
        }
    }
}
