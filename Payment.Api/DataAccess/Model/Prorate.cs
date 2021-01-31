using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Api.DataAccess.Model
{
    [Table("Prorate")]
    public class Prorate
    {
        [Column("ProrateID")]
        public long Id { get; set; }
        [Column("SubscriptionID")]
        public long SubscriptionId { get; set; }
        [Column("PaymentGatewayInvoiceID")]
        public string PaymentGatewayInvoiceId { get; set; }

        public DateTime CreatedOn { get; set; }
        public long Amount { get; set; }
        public bool Refunded { get; set; }

        public Subscription Subscription { get; set; }

    }
}
