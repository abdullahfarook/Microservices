using System.ComponentModel.DataAnnotations;

namespace Payment.Api.ViewModel
{
    public class SubscriptionViewModel
    {
        public long Id { get; set; }
        public virtual int PackageId { get; set; }
        public string Usage { get; set; }
        public virtual int ChargeType { get; set; }
    }
    public class CreateSubscriptionViewModel: SubscriptionViewModel
    {
        [Required]
        public override int PackageId { get; set; }
        [Required]
        public override int ChargeType { get; set; }
        public string Coupon { get; set; }
    }

    public class SubscriptionViewModelWithProrateAmount : SubscriptionViewModel
    {
        public long Amount { get; set; }
    }
}
