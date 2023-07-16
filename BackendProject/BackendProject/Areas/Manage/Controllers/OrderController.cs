using BackendProject.Areas.Manage.ViewModels;
using BackendProject.DAL;
using BackendProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.Areas.Manage.Controllers
{
    //public class OrderController : Controller
    //{
    //    private readonly ProjectDbContext _context;
    //    private readonly IHubContext<PustokHub> _hubContext;
    //    public OrderController(ProjectDbContext context, IHubContext<PustokHub> hubContext)
    //    {
    //        _context = context;
    //        _hubContext = hubContext;
    //    }
    //    public IActionResult Index(int page = 1)
    //    {
    //        var query = _context.Orders.Include(x => x.OrderItems).AsQueryable();
    //        return View(PaginatedList<Order>.Create(query, page, 4));
    //    }

    //    public IActionResult Edit(int id)
    //    {
    //        Order order = _context.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Product).FirstOrDefault(x => x.Id == id);

    //        if (order == null) return View("error");

    //        return View(order);
    //    }

    //    public async Task<IActionResult> Accept(int id)
    //    {
    //        Order order = _context.Orders.Include(x => x.AppUser).FirstOrDefault(x => x.Id == id);

    //        if (order == null || order.Status != Enums.OrderStatus.Pending) return View("error");

    //        order.Status = Enums.OrderStatus.Accepted;
    //        _context.SaveChanges();

    //        if (order.AppUser != null && order.AppUser.IsOnline)
    //        {
    //            await _hubContext.Clients.Client(order.AppUser.ConnectionId).SendAsync("SetOrderStatus", true);
    //        }

    //        return RedirectToAction("Index");
    //    }

    //    public async Task<IActionResult> Reject(int id)
    //    {
    //        Order order = _context.Orders.Include(x => x.AppUser).FirstOrDefault(x => x.Id == id);

    //        if (order == null || order.Status != Enums.OrderStatus.Pending) return View("error");

    //        order.Status = Enums.OrderStatus.Rejected;
    //        _context.SaveChanges();

    //        if (order.AppUser != null && order.AppUser.IsOnline)
    //        {
    //            await _hubContext.Clients.Client(order.AppUser.ConnectionId).SendAsync("SetOrderStatus", false);
    //        }

    //        return RedirectToAction("Index");
    //    }

    //}
}
