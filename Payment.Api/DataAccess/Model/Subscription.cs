using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Api.DataAccess.Model
{
    [Table("Subscription")]
    public class Subscription
    {
        [Column("SubscriptionID")]
        public long Id { get; set; }
        [Column("PaymentGatewaySubscriptionID")]
        public string PaymentGatewaySubscriptionId { get; set; }
        [Column("PackageID")]
        public int PackageId { get; set; }
        [Column("CustomerID")]
        public long CustomerId { get; set; }
        public string Usage { get; set; }
        public Customer Customer { get; set; }
        public Package Package { get; set; }
        public bool IsDeleted { get; set; }
        public int ChargeType { get; set; }

        public Prorate Prorate { get; set; }
    }
}
