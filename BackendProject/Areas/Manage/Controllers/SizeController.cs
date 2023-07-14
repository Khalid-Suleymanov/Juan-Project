using BackendProject.Areas.Manage.ViewModels;
using BackendProject.DAL;
using BackendProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]

    [Area("manage")]
    public class SizeController : Controller
    {
        private readonly ProjectDbContext _context;
        public SizeController(ProjectDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Sizes.AsQueryable();
            return View(PaginatedList<Size>.Create(query, page, 2));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Size size)
        {
            if (!ModelState.IsValid)
                return View();
            if (_context.Sizes.Any(x => x.Name == size.Name))
            {
                ModelState.AddModelError("Name", "Size is already taken");
                return View();
            }
            _context.Sizes.Add(size);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Edit(int id)
        {
            Size size = _context.Sizes.FirstOrDefault(x => x.Id == id);
            if (size == null) return View("error");
            return View(size);
        }
        [HttpPost]
        public IActionResult Edit(Size size)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Size existSize = _context.Sizes.FirstOrDefault(x => x.Id == size.Id);
            if (existSize == null) return View("error");
            if (size.Name != existSize.Name && _context.Sizes.Any(x => x.Name == size.Name))
            {
                ModelState.AddModelError("Name", "Size is already taken");
                return View();
            }
            existSize.Name = size.Name;
            _context.SaveChanges();
            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Size size = _context.Sizes.Find(id);
            if (size == null) return StatusCode(404);
            //if (_context.Products.Any(x => x.SizeId== id)) return StatusCode(400);
            _context.Sizes.Remove(size);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
