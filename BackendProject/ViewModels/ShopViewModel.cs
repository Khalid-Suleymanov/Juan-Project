using BackendProject.Areas.Manage.ViewModels;
using BackendProject.Enums;
using BackendProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackendProject.ViewModels
{
    public class ShopViewModel
    {
        public PaginatedList<Product> AllProduct { get; set; }
        public List<Product> Product { get; set; }
        public List<Category> Categories { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Size> Sizes { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal SelectedMinPrice { get; set; }
        public decimal SelectedMaxPrice { get; set; }
        public GenderStatus? Status { get; set; }
        public List<int>? SelectedCategoryIds { get; set; }
        public List<int>? SelectedBrandIds { get; set; }
        public List<int> SelectedSizeIds { get; set; }
        public List<SelectListItem> SortItems { get; set; }
    }
}
