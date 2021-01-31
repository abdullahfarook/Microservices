using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Payment.Api.ViewModel
{
    public class PackageViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsSubscribed { get; set; }
        public bool IsPublished { get; set; }
        public List<FeatureViewModel> Features { get; set; }
        [Required]
        public long Amount { get; set; }
        [Required]
        public string Interval { get; set; }
        [Required]
        public long IntervalCount { get; set; }
        public long? TrialPeriodDays { get; set; }
        [Required]
        public string UsageType { get; set; }
    }
}
