using System.Collections.Generic;

namespace Payment.Api.ViewModel
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {
            Packages = new List<PackageViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public List<PackageViewModel> Packages { get; set; }
    }
}
