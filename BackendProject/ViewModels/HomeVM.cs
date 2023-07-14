using BackendProject.Models;

namespace BackendProject.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }

        public List<Feature> Features { get; set; }

        public List<Product> Products { get; set; }

        public List<Product> NewProducts { get; set; }

        public List<FourBrand> FourBrands { get; set; }

        public List<Category> Categories { get; set; }      

        public List<Color> Colors { get; set; }

        public List<Size> Sizes { get; set; }

        public List<Brand> Brands { get; set; }

        public List <SportOff> SportOffs { get; set; }
    }
}
