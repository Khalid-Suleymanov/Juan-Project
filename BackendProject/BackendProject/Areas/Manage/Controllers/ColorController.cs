using BackendProject.Areas.Manage.ViewModels;
using BackendProject.DAL;
using BackendProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendProject.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]

    [Area("manage")]
    public class ColorController : Controller
    {
        private readonly ProjectDbContext _context;
        public ColorController(ProjectDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Colors.AsQueryable();
            return View(PaginatedList<Color>.Create(query, page, 2));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Color color)
        {
            if (!ModelState.IsValid)
                return View();
            if (_context.Colors.Any(x => x.Name == color.Name))
            {
                ModelState.AddModelError("Name", "Name is already taken");
                return View();
            }
            _context.Colors.Add(color);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Edit(int id)
        {
            Color color = _context.Colors.FirstOrDefault(x => x.Id == id);
            if (color == null) return View("error");
            return View(color);
        }
        [HttpPost]
        public IActionResult Edit(Color color)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Color existColor = _context.Colors.FirstOrDefault(x => x.Id == color.Id);
            if (existColor == null) return View("error");
            if (color.Name != existColor.Name && _context.Colors.Any(x => x.Name == color.Name))
            {
                ModelState.AddModelError("Name", "Name is already taken");
                return View();
            }
            existColor.Name = color.Name;
            _context.SaveChanges();
            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Color color = _context.Colors.Find(id);
            if (color == null) return StatusCode(404);
            if (_context.Products.Any(x => x.ColorId == id)) return StatusCode(400);
            _context.Colors.Remove(color);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
