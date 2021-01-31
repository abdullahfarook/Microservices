using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Api.DataAccess.Model
{
    [Table("Coupon")]
    public class Coupon
    {
        [Column("CouponID")]
        public int Id { get; set; }
        [Column("PaymentGatewayCouponID")]
        public string PaymentGatewayCouponId { get; set; }
        public DateTime Created { get; set; }
        [Column("CouponDetail")]
        public string Object { get; set; }
        [Column("CouponName")]
        public string Name { get; set; }
        [Column("CouponValidTill")]
        public DateTime? RedeemBy { get; set; }
        public string Duration { get; set; }
        public long? DurationInMonths { get; set; }
        public int? MaxRedemptions { get; set; }
        public int? TimesRedeemed { get; set; }
        public string Currency { get; set; }
        public long? AmountOff { get; set; }
        public decimal? PercentOff { get; set; }
        public bool Valid { get; set; }
    }
}
