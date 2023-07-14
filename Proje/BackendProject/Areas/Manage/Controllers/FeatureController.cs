using BackendProject.Areas.Manage.ViewModels;
using BackendProject.DAL;
using BackendProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendProject.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]

    [Area("manage")]
    public class FeatureController : Controller
    {
        private readonly ProjectDbContext _context;
        public FeatureController(ProjectDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Features.AsQueryable();
            return View(PaginatedList<Feature>.Create(query, page, 2));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Feature feature)
        {
            if (!ModelState.IsValid)
                return View();
            if (_context.Features.Any(x => x.Title == feature.Title))
            {
                ModelState.AddModelError("Title", "Title is already taken");
                return View();
            }
            _context.Features.Add(feature);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Edit(int id)
        {
            Feature feature = _context.Features.FirstOrDefault(x => x.Id == id);
            if (feature == null) return View("error");
            return View(feature);
        }
        [HttpPost]
        public IActionResult Edit(Feature feature)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Feature existFeature = _context.Features.FirstOrDefault(x => x.Id == feature.Id);
            if (existFeature == null) return View("error");
            if (feature.Title != existFeature.Title && _context.Features.Any(x => x.Title == feature.Title))
            {
                ModelState.AddModelError("Title", "Title is already taken");
                return View();
            }
            existFeature.Title = feature.Title;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Feature feature = _context.Features.Find(id);
            if (feature == null)
            {
                return StatusCode(404);
            }
            _context.Features.Remove(feature);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
