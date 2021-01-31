using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Api.DataAccess.Model
{
    [Table("Card")]
    public class Card
    {
        [Column("CardID")]
        public int Id { get; set; }
        [Column("PaymentGatewayCardID")]
        public string PaymentGatewayCardId { get; set; }
        [Column("CustomerID")]
        public long CustomerId { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Last4 { get; set; }
        public long ExpMonth { get; set; }
        public long ExpYear { get; set; }
        public bool IsDefault { get; set; }
        public DateTime Created { get; set; }
        public string AddressCity { get; set; }
        public string Country { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressZip { get; set; }
        public Customer Customer { get; set; }
    }
}
