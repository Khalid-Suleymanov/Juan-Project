using BackendProject.Areas.Manage.ViewModels;
using BackendProject.DAL;
using BackendProject.Helpers;
using BackendProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]

    [Area("manage")]
    public class ProductController : Controller
    {
        private readonly ProjectDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ProductController(ProjectDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1, string search = null)
        {
            ViewBag.Search = search;
            var query = _context.Products
                .Include(x => x.Category)
                .Include(x=>x.Brand)
                .Include(x => x.Color)
                .Include(x=>x.Images.Where(p=>p.ImageStatus==true))
                .Include(x=>x.ProductSizes)
                .ThenInclude(p=>p.Size).AsQueryable();
            if(search != null)
            {
                query = query.Where(p => p.Name.Contains(search));
            }
            return View(PaginatedList<Product>.Create(query,page,2));
        }
        public IActionResult Create()
        {
            ViewBag.Brands = _context.Brands.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Colors = _context.Colors.ToList();
            ViewBag.Sizes = _context.Sizes.ToList();

            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Brands = _context.Brands.ToList();
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Colors = _context.Colors.ToList();
                ViewBag.Sizes = _context.Sizes.ToList();
                return View();
            }
            if(!_context.Brands.Any(p=>p.Id == product.BrandId))
            {
                return View("Error");
            }
            if (!_context.Categories.Any(c => c.Id == product.CategoryId))
            {
                return View("Error");
            }
            if (!_context.Colors.Any(c => c.Id == product.ColorId))
            {
                return View("Error");
            }
            if (product.ImageFile == null)
            {
                return View("Error");
            }
            if (product.ImageFiles == null)
            {
                return View("Error");
            }
            Image StatImg = new Image
            {
                ImageStatus = true,
                ImageName = UpFileManage.Save(product.ImageFile, _env.WebRootPath, "Manage/Uploads/Products")
            };
            foreach (var item in product.SizeIds)
            {
                if(!_context.Sizes.Any(s=>s.Id == item))
                {
                    return View("Error");
                }
                product.ProductSizes.Add(new ProductSize
                {
                    SizeId = item
                });
            }
            product.Images.Add(StatImg);
            foreach (var item in product.ImageFiles)
            {
                Image image = new Image()
                {
                    ImageStatus = false,
                    ImageName = UpFileManage.Save(item, _env.WebRootPath, "Manage/Uploads/Products")
                };
                product.Images.Add(image);
            }
            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var existProduct = _context.Products
                .Include(x => x.Images)
                .Include(x => x.ProductSizes)
                .FirstOrDefault(x => x.Id == id);
            if (existProduct == null)
            {
                return View("Error");
            }
            existProduct.SizeIds = existProduct.ProductSizes.Select(x=>x.SizeId).ToList();
            ViewBag.Brands = _context.Brands.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Colors = _context.Colors.ToList();
            ViewBag.Sizes = _context.Sizes.ToList();
            return View(existProduct);
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                var exProduct = _context.Products
                    .Include(x=>x.Images)
                    .Include(x=>x.ProductSizes)
                    .FirstOrDefault(x=>x.Id == product.Id);
                if(exProduct == null)
                {
                    return View("Error");
                }
                exProduct.SizeIds = exProduct.ProductSizes.Select(x => x.SizeId).ToList();
                ViewBag.Brands = _context.Brands.ToList();
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Colors = _context.Colors.ToList();
                ViewBag.Sizes = _context.Sizes.ToList();
                return View(exProduct);
            }
            var existProd = _context.Products
                .Include(x => x.Images)
                .Include(x => x.ProductSizes)
                .FirstOrDefault(x => x.Id == product.Id);
            if(existProd == null)
            {
                return View("Error");
            }
            if (!_context.Brands.Any(p => p.Id == product.BrandId))
            {
                return View("Error");
            }
            if (!_context.Categories.Any(c => c.Id == product.CategoryId))
            {
                return View("Error");
            }
            if (!_context.Colors.Any(c => c.Id == product.ColorId))
            {
                return View("Error");
            }
            existProd.ProductSizes = new List<ProductSize>();
            foreach (var item in product.SizeIds)
            {
                if (!_context.Sizes.Any(s => s.Id == item))
                {
                    return View("Error");
                }
                existProd.ProductSizes.Add(new ProductSize
                {
                    SizeId = item
                });
            }
            existProd.Name= product.Name;
            existProd.Description = product.Description;
            existProd.StockStatus = product.StockStatus;
            existProd.SalePrice = product.SalePrice;
            existProd.CostPrice = product.CostPrice;
            existProd.CategoryId = product.CategoryId;
            existProd.ColorId = product.ColorId;
            existProd.BrandId = product.BrandId;
            existProd.DiscountedPrice = product.DiscountedPrice;
            existProd.IsNew= product.IsNew;
            List<string> removableAllImages = new List<string>();
            if(product.ImageFile!= null)
            {
                Image postStatus = existProd.Images.FirstOrDefault(x=>x.ImageStatus==true);
                removableAllImages.Add(postStatus.ImageName);
                postStatus.ImageName = UpFileManage.Save(product.ImageFile, _env.WebRootPath, "Manage/Uploads/Products");
            }
            var removableImages = existProd.Images.FindAll(x => x.ImageStatus == false);
            _context.Images.RemoveRange(removableImages);
            removableAllImages.AddRange(removableImages.Select(x => x.ImageName).ToList());
            foreach (var item in product.ImageFiles)
            {
                Image image = new Image()
                {
                    ImageStatus = false,
                    ImageName = UpFileManage.Save(item, _env.WebRootPath, "Manage/Uploads/Products")
                };
                existProd.Images.Add(image);
            }
            _context.SaveChanges();
            UpFileManage.DeleteAll(_env.WebRootPath, "Manage/Uploads/Products", removableAllImages);
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var existProduct = _context.Products.FirstOrDefault(x => x.Id == id);
            if(existProduct == null)
            {
                return View("Error");
            }
            List<string> removableImages = new List<string>();
            var image = existProduct.Images.Select(x => x.ImageName);
            removableImages.AddRange(image);
            _context.Products.Remove(existProduct);
            _context.SaveChanges();
            UpFileManage.DeleteAll(_env.WebRootPath, "Manage/Uploads/Products", removableImages);
            return RedirectToAction("Index");
        }
    }
}
