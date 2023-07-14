using BackendProject.Areas.Manage.ViewModels;
using BackendProject.DAL;
using BackendProject.Enums;
using BackendProject.Models;
using BackendProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.Controllers
{
    public class ShopController : Controller
    {
        private readonly ProjectDbContext _context;

        public ShopController(ProjectDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(List<int>? categoryId = null, List<int>? brandId = null, List<int>? sizeId = null, decimal? minPrice = null, GenderStatus? status = null, decimal? maxPrice = null, string sort = "A_to_Z", int page = 1)
        {
            ShopViewModel vm = new ShopViewModel();
            var query = _context.Products.Include(x => x.Images.Where(x => x.ImageStatus != null)).Include(x => x.Category).AsQueryable();
            vm.MinPrice = query.Min(x => x.SalePrice);
            vm.MaxPrice = query.Max(x => x.SalePrice);
            if (categoryId.Count >0)
            {
                query = query.Where(x => categoryId.Contains(x.CategoryId));
            }
            if (brandId.Count > 0)
            {
                query = query.Where(x => brandId.Contains(x.BrandId));
            }
            if (sizeId.Count > 0)
            {
                query = query.Where(x => x.ProductSizes.Any(x=> sizeId.Contains(x.SizeId)));
            }
            if (minPrice != null && maxPrice != null)
            {
                query = query.Where(x => x.SalePrice >= minPrice && x.SalePrice <= maxPrice);
            }
            switch (sort)
            {
                case "Z_to_A":
                    query = query.OrderByDescending(x => x.Name);
                    break;
                case "Low_to_High":
                    query = query.OrderBy(x => x.SalePrice);
                    break;
                case "High_to_Low":
                    query = query.OrderByDescending(x => x.SalePrice);
                    break;
                default:
                    query = query.OrderBy(x => x.Name);
                    break;
            }
            vm.Product = _context.Products.ToList();
            vm.AllProduct = PaginatedList<Product>.Create(query, page, 9);
            vm.Categories = _context.Categories.Include(x => x.Products).ToList();
            vm.Brands = _context.Brands.Include(x => x.Products).ToList();
            vm.Sizes = _context.Sizes.Include(x => x.ProductSizes).ToList();
            vm.Status = status;
            vm.SelectedCategoryIds = categoryId;
            vm.SelectedBrandIds = brandId;
            vm.SelectedSizeIds = sizeId;       
            vm.SelectedMinPrice = minPrice == null ? vm.MinPrice : (decimal)minPrice;
            vm.SelectedMaxPrice = maxPrice == null ? vm.MaxPrice : (decimal)maxPrice;
            vm.SortItems = new List<SelectListItem>
            {
                new SelectListItem("Sort by: (A-Z)","A_to_Z",sort=="A_to_Z"),
                new SelectListItem("Sort by: (Z-A)","Z_to_A",sort=="Z_to_A"),
                new SelectListItem("Sort by: (Low-High)","Low_to_High",sort=="Low_to_High"),
                new SelectListItem("Sort by: (High-Low)","High_to_Low",sort=="High_to_Low"),
            };
           
            return View(vm);
        }
    }
}

