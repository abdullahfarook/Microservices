using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Api.DataAccess.Model
{
    [Table("Package")]
    public class Package
    {
        [Column("PackageID")]
        public int Id { get; set; }
        [Column("ProductID")]
        public int ProductId { get; set; }
        [Column("PaymentGatewayPackageID")]
        public string PaymentGatewayPackageId { get; set; }
        [Column("PackageName")]
        public string Name { get; set; }
        public long Amount { get; set; }
        public string Interval { get; set; }
        public long IntervalCount { get; set; }
        public string UsageType { get; set; }
        public string Description { get; set; }
        public bool IsSubscribed { get; set; }
        public bool IsPublished { get; set; }
        public string Features { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public Product Product { get; set; }
    }
}
