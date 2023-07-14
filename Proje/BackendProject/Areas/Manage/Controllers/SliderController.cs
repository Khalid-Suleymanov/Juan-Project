using BackendProject.Areas.Manage.ViewModels;
using BackendProject.Helpers;
using BackendProject.DAL;
using BackendProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Humanizer.Localisation;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;

namespace BackendProject.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]

    [Area("manage")]
    public class SliderController : Controller
    {
        private readonly ProjectDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SliderController(ProjectDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Sliders.AsQueryable();
            return View(PaginatedList<Slider>.Create(query, page, 2));
        } 
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if (!ModelState.IsValid) return View();
            if (slider.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "ImageFile is required");
                return View();
            }
            if (slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/png" && slider.ImageFile.ContentType != "image/jpg")
            {
                ModelState.AddModelError("ImageFile", "ImageFile must be .jpg,.jpeg or .png");
                return View();
            } 
            slider.Image = UpFileManage.Save(slider.ImageFile, _env.WebRootPath, "Manage/Uploads/Sliders");
            _context.Sliders.Add(slider);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Edit(int id)
        {
            Slider slider = _context.Sliders.Find(id);
            if (slider == null) return View("Error");
            return View(slider);
        }
        [HttpPost]
        public IActionResult Edit(Slider slider)
        {
            if (!ModelState.IsValid) { return View(slider); }
            Slider existSlider = _context.Sliders.Find(slider.Id);
            if (existSlider == null) return View("Error");
            string removableImageName = null;
            if (slider.ImageFile != null)
            {
                if (slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/png" && slider.ImageFile.ContentType != "image/jng")
                {
                    ModelState.AddModelError("ImageFile", "ImageFile must be .jpg,.jpeg or .png");
                    return View(slider);
                }
                removableImageName = existSlider.Image;
                existSlider.Image = UpFileManage.Save(slider.ImageFile, _env.WebRootPath, "Manage/Uploads/Sliders");
            }
            existSlider.Title1 = slider.Title1;
            existSlider.Title2 = slider.Title2;
            existSlider.Desc = slider.Desc;
            existSlider.BtnText = slider.BtnText;
            existSlider.BtnUrl = slider.BtnUrl;
            existSlider.Order = slider.Order;
            _context.SaveChanges();
            if (removableImageName != null) UpFileManage.Delete(_env.WebRootPath, "Manage/Uploads/Sliders", removableImageName);
            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            var existSlider = _context.Sliders.Find(id);
            if(existSlider == null)
            {
                return View("Error");
            }
            var removableImage = existSlider.Image;
            _context.Sliders.Remove(existSlider);
            _context.SaveChanges();

            UpFileManage.Delete(_env.WebRootPath, "Manage/Uploads/Sliiders", removableImage);
            return RedirectToAction("index");
        }
    }
}
