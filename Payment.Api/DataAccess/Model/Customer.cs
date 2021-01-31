using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Api.DataAccess.Model
{
    [Table("Customer")]
    public class Customer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Column("CustomerID")]
        public long Id { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Pic { get; set; }
        public string Email { get; set; }
        [Column("PaymentGatewayCustomerID")]
        public string PaymentGatewayCustomerId { get; set; }
        [Column("PaymentGatewayDefaultCardID")]
        public string PaymentGatewayDefaultCardId { get; set; }
    }
}
