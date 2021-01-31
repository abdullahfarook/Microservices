using System;
using System.ComponentModel.DataAnnotations;

namespace Payment.Api.ViewModel
{
    public class InvoiceViewModel
    {

        public string Id { get; set; }
        public DateTime Created { get; set; }
        public string Description { get; set; }
        public string Number { get; set; }
        public long AmountDue { get; set; }
        public long AmountPaid { get; set; }
        public string Status { get; set; }
        public string InvoicePdf { get; set; }
        public string Currency { get; set; }
        public long Subtotal { get; set; }
        public string CollectionMethod { get; set; }
        public string ChargeId { get; set; }
        public bool Refunded { get; set; }
        public CustomerViewModel Customer { get; set; }
    }

    public class RefundInvoiceViewModel
    {
        [Required]
        public string InvoiceId { get; set; }
        public bool Prorate { get; set; }

    }
}
