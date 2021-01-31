using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Payment.Api.ViewModel
{
    public class FeatureViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Key { get; set; }
        public  string Value { get; set; }
        [Required]
        public string DataType { get; set; }
        public bool ShowInSummery { get; set; }
    }

    public class FeatureTypeViewModel
    {
        public FeatureTypeViewModel()
        {
            Features = new List<FeatureViewModel>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<FeatureViewModel> Features { get; set; }
    }
}
