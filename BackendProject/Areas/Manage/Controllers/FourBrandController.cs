using BackendProject.Areas.Manage.ViewModels;
using BackendProject.DAL;
using BackendProject.Helpers;
using BackendProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendProject.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]

    [Area("manage")]
    public class FourBrandController : Controller
    {
        private readonly ProjectDbContext _context;
        private readonly IWebHostEnvironment _env;
        public FourBrandController(ProjectDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.FourBrands.AsQueryable();
            return View(PaginatedList<FourBrand>.Create(query, page, 2));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(FourBrand fourBrand)
        {
            if (!ModelState.IsValid) return View();
            if (fourBrand.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "ImageFile is required");
                return View();
            }
            if (fourBrand.ImageFile.ContentType != "image/jpeg" && fourBrand.ImageFile.ContentType != "image/png" && fourBrand.ImageFile.ContentType != "image/jpg")
            {
                ModelState.AddModelError("ImageFile", "ImageFile must be .jpg,.jpeg or .png");
                return View();
            }
            fourBrand.Image = UpFileManage.Save(fourBrand.ImageFile, _env.WebRootPath, "Manage/Uploads/Products");
            _context.FourBrands.Add(fourBrand);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Edit(int id)
        {
            FourBrand fourBrand = _context.FourBrands.Find(id);
            if (fourBrand == null) return View("Error");
            return View(fourBrand);
        }
        [HttpPost]
        public IActionResult Edit(FourBrand fourBrand)
        {
            if (!ModelState.IsValid) { return View(fourBrand); }
            FourBrand existFBrand = _context.FourBrands.Find(fourBrand.Id);
            if (existFBrand == null) return View("Error");
            string removableImageName = null;
            if (fourBrand.ImageFile != null)
            {
                if (fourBrand.ImageFile.ContentType != "image/jpeg" && fourBrand.ImageFile.ContentType != "image/png" && fourBrand.ImageFile.ContentType != "image/jng")
                {
                    ModelState.AddModelError("ImageFile", "ImageFile must be .jpg,.jpeg or .png");
                    return View(fourBrand);
                }
                removableImageName = existFBrand.Image;
                existFBrand.Image = UpFileManage.Save(fourBrand.ImageFile, _env.WebRootPath, "Manage/Uploads/Products");
            }
            _context.SaveChanges();
            if (removableImageName != null) UpFileManage.Delete(_env.WebRootPath, "Manage/Uploads/Products", removableImageName);
            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            var existFBrand = _context.FourBrands.Find(id);
            if (existFBrand == null)
            {
                return View("Error");
            }
            var removableImage = existFBrand.Image;
            _context.FourBrands.Remove(existFBrand);
            _context.SaveChanges();

            UpFileManage.Delete(_env.WebRootPath, "Manage/Uploads/Products", removableImage);
            return RedirectToAction("index");
        }
    }
}
