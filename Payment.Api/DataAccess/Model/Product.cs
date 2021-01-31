using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Api.DataAccess.Model
{
    [Table("Product")]
    public class Product
    {
        public Product()
        {
            Packages = new HashSet<Package>();
        }
        [Column("ProductID")]
        public int Id { get; set; }
        [Column("PaymentGatewayProductID")]
        public string PaymentGatewayProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public ICollection<Package> Packages { get; set; }

    }

}
