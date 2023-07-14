using BackendProject.Areas.Manage.ViewModels;
using BackendProject.DAL;
using BackendProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Security.Policy;

namespace BackendProject.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]

    [Area("manage")]
    public class CategoryController : Controller
    {
        private readonly ProjectDbContext _context;
        public CategoryController(ProjectDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Categories.AsQueryable();
            return View(PaginatedList<Category>.Create(query,page,2));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_context.Categories.Any(x => x.Name == category.Name))
            {
                ModelState.AddModelError("Name", "Category is already taken");
                return View();
            }
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Edit(int id)
        {
            Category category = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null) return View("error");
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Category existCategory = _context.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (existCategory == null) return View("error");
            if (category.Name != existCategory.Name && _context.Categories.Any(x => x.Name == category.Name))
            {
                ModelState.AddModelError("Name", "Category is already taken");
                return View();
            }
            existCategory.Name = category.Name;
            _context.SaveChanges();
            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Category category = _context.Categories.Find(id);
            if (category == null)
            {
                return StatusCode(404);
            }
            if (_context.Products.Any(x => x.CategoryId == id))
            {
                return StatusCode(400);
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
