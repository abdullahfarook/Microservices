using System;

namespace Payment.Api.ViewModel
{
    public class CouponViewModel
    {
        public int Id { get; set; }
        public string PaymentGatewayCouponId { get; set; }
        public DateTime Created { get; set; }
        public string Object { get; set; }
        public string Name { get; set; }
        public DateTime? RedeemBy { get; set; }
        public bool Valid { get; set; }
        public int? MaxRedemptions { get; set; }
        public int? TimesRedeemed { get; set; }
        public long? AmountOff { get; set; }
        public decimal? PercentOff { get; set; }
    }
    public class CreateCouponViewModel : CouponViewModel
    {
        public string? Duration { get; set; }
        public long? DurationInMonths { get; set; }
        public string? Currency { get; set; }
    }
}
