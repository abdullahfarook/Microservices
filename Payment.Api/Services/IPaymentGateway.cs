using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Payment.Api.Core;
using Payment.Api.ViewModel;
using Stripe;

namespace Payment.Api.Services
{
    public class PaymentGatewayError : IPaymentGatewayError
    {
        public string Code { get; set; }
        public string Error { get; set; }
        public bool Succeeded { get; set; }
        public Exception Exception { get; set; }
    }
    public partial class PaymentGatewayResult<T> : PaymentGatewayError
    {
        public T Result { get; set; }
        public static PaymentGatewayResult<T> Success(T result)
        {
            return new PaymentGatewayResult<T>
            {
                Succeeded = true,
                Result = result
            };

        }
        public static PaymentGatewayResult<T> Failed(HttpStatusCode code, string message)
        {
            return new PaymentGatewayResult<T>
            {
                Succeeded = false,
                Code = code.ToString(),
                Error = message
            };

        }
        public static PaymentGatewayResult<T> Failed(StripeException error)
        {
            return new PaymentGatewayResult<T>
            {
                Succeeded = false,
                Code = error.HttpStatusCode.ToString(),
                Error = error.Message
            };

        }
    }
    public interface IPaymentGatewayError
    {
        string Code { get; set; }
        string Error { get; set; }
        bool Succeeded { get; set; }
        Exception Exception { get; set; }
    }
    public interface IPaymentProduct
    {
        string Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        Dictionary<string, string> Metadata { get; set; }
        string Type { get; set; }
        bool? Active { get; set; }
        DateTime Created { get; set; }
        bool? Deleted { get; set; }
    }
    public interface IPaymentPackage
    {
        string Id { get; set; }
        string Name { get; set; }
        long? Amount { get; set; }
        string Interval { get; set; }
        long IntervalCount { get; set; }
        long? TrialPeriodDays { get; set; }
        string UsageType { get; set; }
        string AggregateUsage { get; set; }

        Dictionary<string, string> Metadata { get; set; }
        bool Active { get; set; }
        DateTime Created { get; set; }
        bool? Deleted { get; set; }
    }
    public interface IPaymentCard
    {
        string Id { get; set; }
        string Name { get; set; }
        string Brand { get; set; }
        string Last4 { get; set; }
        long ExpMonth { get; set; }
        long ExpYear { get; set; }
        bool IsDefault { get; set; }
        DateTime Created { get; set; }
        string AddressCity { get; set; }
        string Country { get; set; }
        string AddressLine1 { get; set; }
        string AddressLine2 { get; set; }
        string AddressZip { get; set; }
    }
    public interface IPaymentCoupon
    {
        string Id { get; set; }
        DateTime Created { get; set; }
        string Object { get; set; }
        string Duration { get; set; }
        long? DurationInMonths { get; set; }
        int? MaxRedemptions { get; set; }
        string Name { get; set; }
        string Currency { get; set; }
        long? AmountOff { get; set; }
        decimal? PercentOff { get; set; }
        DateTime? RedeemBy { get; set; }
        int TimesRedeemed { get; set; }
        bool Valid { get; set; }
    }

    public interface IPaymentSubscription
    {
        string Id { get; set; }
        string? CollectionMethod { get; set; }
        string Customer { get; set; }
        DateTime Created { get; set; }
        string LatestInvoiceId { get; set; }
    }
    public interface IPaymentInvoice
    {
        string Id { get; set; }
        DateTime Created { get; set; }
        string Description { get; set; }
        string Number { get; set; }
        long AmountDue { get; set; }
        long AmountPaid { get; set; }
        string Status { get; set; }
        string InvoicePdf { get; set; }
        long Subtotal { get; set; }
        string Currency { get; set; }
        string CollectionMethod { get; set; }
        string CustomerId { get; set; }
        string ChargeId { get; set; }
        string HostedInvoiceUrl { get; set; }
        string SubscriptionId { get; set; }
        IPaymentCharge Charge { get; set; }
    }
    public interface IPaymentRefund
    {
        string Id { get; set; }
        DateTime Created { get; set; }
        string Description { get; set; }
        long Amount { get; set; }
        string Status { get; set; }
        string ChargeId { get; set; }
    }

    public interface IPaymentCustomer
    {
        string Id { get; set; }
        string Name { get; set; }
        long AccountBalance { get; set; }
        Address Address { get; set; }
        long Balance { get; set; }
        DateTime Created { get; set; }
        string DefaultSourceId { get; set; }
    }

    public interface IPaymentCharge
    {
        string Id { get; set; }
        long Amount { get; set; }
        DateTime Created { get; set; }
        bool Refunded { get; set; }
    }

    //public class ChargeType
    //{
    //    public static string Automatic { get; } = "charge_automatically";
    //    public static string Manual { get; } = "send_invoice";
    //}

    public interface IPaymentGateway
    {
        Task<IPaymentProduct> GetProductByIdAsync(string productId);
        Task<List<IPaymentProduct>> GetProductsAsync(long? pageSize = null, bool? active = null, List<string> productIds = null);
        Task<IPaymentProduct> InsertProductAsync(IPaymentProduct product);
        Task<IPaymentProduct> UpdateProductAsync(string productId, IPaymentProduct product);
        Task<IPaymentProduct> DeleteProductAsync(string productId);
        Task<List<IPaymentPackage>> GetPackagesByProductAsync(string productId, bool? active = null);
        Task<IPaymentPackage> InsertPackageAsync(string productId, IPaymentPackage package);
        Task<IPaymentPackage> DeletePackageAsync(string packageId);
        string GetPublishableKey();
        Task<List<IPaymentCard>> GetCardsAsync(string customerId);
        Task<List<IPaymentInvoice>> GetInvoicesAsync(string? customerId = null, DateRangeOptions? dateTimeOptions = null, long? pageSize = null);
        Task<IPaymentInvoice> GetInvoiceAsync(string invoiceId);
        Task<IPaymentInvoice> GetSubscriptionByInvoiceAsync(string invoiceId);
        Task<IPaymentCustomer> SetDefaultCardAsync(string paymentCustomerId, string paymentCardId);
        Task<PaymentGatewayResult<IPaymentCard>> GetCardAsync(string paymentCustomerId, string paymentCardId);
        Task<PaymentGatewayResult<IPaymentCard>> CreateCardAsync(string paymentCustomerId, string paymentCardToken);
        Task<PaymentGatewayResult<IPaymentCard>> UpdateCardAsync(string paymentCustomerId, IPaymentCard card);
        Task<PaymentGatewayResult<IPaymentCard>> DeleteCardAsync(string paymentCustomerId, string paymentCardId);
        Task<PaymentGatewayResult<IPaymentCustomer>> CreateCustomerAsync(string email, string paymentCardToken);
        Task<PaymentGatewayResult<List<IPaymentCoupon>>> GetAllCouponsAsync();
        Task<PaymentGatewayResult<IPaymentCoupon>> CreateCouponAsync(CreateCouponViewModel createCouponVm);
        Task<PaymentGatewayResult<IPaymentCoupon>> GetCouponByIdAsync(string Id);
        Task<PaymentGatewayResult<IPaymentCoupon>> UpdateCouponAsync(string Id, CouponUpdateOptions options);
        Task<PaymentGatewayResult<IPaymentCoupon>> DeleteCouponAsync(string Id);
        Task<PaymentGatewayResult<IPaymentRefund>> RefundChargeAsync(string chargeId,long? amount = null);
        Task<PaymentGatewayResult<List<IPaymentSubscription>>> GetAllSubscriptionsAsync();

        Task<PaymentGatewayResult<IPaymentInvoice>> GetLatestInvoiceBySubscription(string subscriptionId,
            string customerId);
        Task<PaymentGatewayResult<IPaymentSubscription>> GetSubscriptionByIdAsync(string id);

        Task<PaymentGatewayResult<IPaymentSubscription>> CreateSubscriptionAsync(string planId, string customerId, ChargeType type,
            string? coupon = null);

        Task<PaymentGatewayResult<IPaymentSubscription>> UpdateSubscriptionAsync(string subscriptionId, ChargeType type, string planId,
            string? coupon);
        Task<PaymentGatewayResult<IPaymentSubscription>> CancelSubscriptionAsync(string subscriptionId, bool? invoiceNow = null, bool? prorate = null);
        Task<long> CalculateProrate(string invoiceId);
        Task<long> CalculateProrate(string subscriptionId, string customerId);
    }
    public class StripePaymentGateway : IPaymentGateway
    {
        private readonly ProductService _productService;
        private readonly IMapper _mapper;
        private readonly PlanService _planService;
        private readonly AppSettings _appSettings;
        private readonly CardService _cardService;
        private readonly InvoiceService _invoiceService;
        private readonly CustomerService _customerSer;
        private readonly CouponService _couponService;
        private readonly RefundService _refundService;
        private readonly SubscriptionService _subscriptionService;
        private readonly InvoiceItemService _invoiceItemService;
        private readonly string[] _chargeTypes = { "charge_automatically", "send_invoice" };

        public StripePaymentGateway(ProductService productService, CustomerService customerService, PlanService planService,
            CardService cardService, InvoiceService invoiceService, CouponService couponService, IMapper mapper, RefundService refundService,
            SubscriptionService subscriptionService, InvoiceItemService invoiceItemService,
            IOptions<AppSettings> appSettings)
        {
            _productService = productService;
            _mapper = mapper;
            _planService = planService;
            _appSettings = appSettings.Value;
            _cardService = cardService;
            _invoiceService = invoiceService;
            _customerSer = customerService;
            _couponService = couponService;
            _refundService = refundService;
            _subscriptionService = subscriptionService;
            _invoiceItemService = invoiceItemService;
        }

        public async Task<List<IPaymentProduct>> GetProductsAsync(long? pageSize = null, bool? active = null, List<string> productIds = null)
        {
            var apiProducts = await _productService
                .ListAsync(new ProductListOptions
                {
                    Limit = pageSize,
                    Active = active,
                    Ids = productIds

                });
            return apiProducts
                .Select(x => _mapper.Map<IPaymentProduct>(x))
                .ToList();
        }
        public async Task<IPaymentProduct> GetProductByIdAsync(string productId)
        {
            if (string.IsNullOrEmpty(productId)) throw new Exception("Product id not provided");
            var product = await _productService.GetAsync(productId);
            return _mapper.Map<IPaymentProduct>(product);
        }
        public async Task<IPaymentProduct> InsertProductAsync(IPaymentProduct product)
        {
            var result = await _productService.CreateAsync(_mapper.Map<ProductCreateOptions>(product));
            return _mapper.Map<IPaymentProduct>(result);
        }

        public async Task<IPaymentProduct> UpdateProductAsync(string productId, IPaymentProduct product)
        {
            if (string.IsNullOrEmpty(productId)) throw new Exception("Product id not provided");
            await _productService.UpdateAsync(productId, _mapper.Map<ProductUpdateOptions>(product));
            return product;
        }

        public async Task<IPaymentProduct> DeleteProductAsync(string productId)
        {
            var product = await _productService.DeleteAsync(productId);
            return _mapper.Map<IPaymentProduct>(product);
        }

        public async Task<List<IPaymentPackage>> GetPackagesByProductAsync(string productId, bool? active = null)
        {
            var packages = await _planService.ListAsync(new PlanListOptions
            {
                ProductId = productId,
                Active = active,

            });
            return packages
                .Select(x => _mapper.Map<IPaymentPackage>(x))
                .ToList();
        }

        public async Task<IPaymentPackage> InsertPackageAsync(string productId, IPaymentPackage package)
        {
            var model = _mapper.Map<PlanCreateOptions>(package);
            model.Product = productId;
            model.Currency = "usd";
            model.Amount = package.Amount * 100;
            model.Active = true;
            var pkg = await _planService.CreateAsync(model);
            return _mapper.Map<IPaymentPackage>(pkg);
        }
        public async Task<IPaymentPackage> DeletePackageAsync(string packageId)
        {
            var pkg = await _planService.DeleteAsync(packageId);
            return _mapper.Map<IPaymentPackage>(pkg);
        }

        public string GetPublishableKey()
        {
            return _appSettings.Stripe.PublishableKey;
        }

        public async Task<List<IPaymentCard>> GetCardsAsync(string customerId)
        {
            var cards = await _cardService.ListAsync(customerId);
            return _mapper.Map<List<IPaymentCard>>(cards);
        }

        public async Task<List<IPaymentInvoice>> GetInvoicesAsync(string? customerId = null, DateRangeOptions? dateTimeOptions = null, long? pageSize = null)
        {
            var optionsInvoice = new InvoiceListOptions
            {
                Limit = pageSize,
                CustomerId = customerId,
                Created = dateTimeOptions,
                Expand = new List<string> { "data.charge" }
            };
            var invoices = await _invoiceService.ListAsync(optionsInvoice);
            foreach (var inv in invoices)
            {
                if (!string.IsNullOrEmpty(inv.ChargeId))
                {
                    if (inv.Charge.AmountRefunded != 0)
                    {
                        if (inv.Charge.AmountRefunded != inv.Charge.Amount)
                        {
                            inv.Status = "partial_refunded";
                        }
                        
                    }
                }
            }
            return _mapper.Map<List<IPaymentInvoice>>(invoices);

        }

        public async Task<IPaymentInvoice> GetInvoiceAsync(string invoiceId)
        {
            var invoice = await _invoiceService.GetAsync(invoiceId, new InvoiceGetOptions
            {
                Expand = new List<string> { "charge" }
            });
            return _mapper.Map<IPaymentInvoice>(invoice);
        }
        public async Task<IPaymentInvoice> GetSubscriptionByInvoiceAsync(string invoiceId)
        {
            var invoice = await _invoiceService.GetAsync(invoiceId);
            return _mapper.Map<IPaymentInvoice>(invoice);
        }
        public async Task<PaymentGatewayResult<List<IPaymentCoupon>>> GetAllCouponsAsync()
        {
            try
            {
                var options = new CouponListOptions
                {
                    Limit = 100,
                };
                var coupons = await _couponService.ListAsync(options);

                foreach (var item in coupons.Data)
                {
                    if (item.PercentOff != null)
                    {

                        if (item.Duration == "once")
                        {
                            item.Object = item.PercentOff + "% off " + item.Duration;
                        }
                        else if (item.Duration == "forever")
                        { item.Object = item.PercentOff + "% off " + item.Duration; }

                        else if (item.Duration == "repeating")
                        {
                            if (item.DurationInMonths == 1) item.Object = item.PercentOff + "% off every Year for 1 month";
                            else item.Object = item.PercentOff + "% off every Year for " + item.DurationInMonths + " months";
                        }

                    }

                    if (item.AmountOff != null)
                    {
                        item.AmountOff = item.AmountOff / 100;
                        if (item.Duration == "once")
                        {
                            item.Object = "$" + item.AmountOff + " off once";
                        }

                        else if (item.Duration == "forever")
                        { item.Object = "$" + item.AmountOff + " off " + item.Duration; }

                        else if (item.Duration == "repeating")
                        {
                            if (item.DurationInMonths == 1) item.Object = "$" + item.AmountOff + " off every Year for 1 month";
                            else item.Object = "$" + item.AmountOff + " off every Year for " + item.DurationInMonths + " months";
                        }
                    }

                }
                return PaymentGatewayResult<List<IPaymentCoupon>>.Success(_mapper.Map<List<IPaymentCoupon>>(coupons));
            }
            catch (StripeException e)
            {
                return PaymentGatewayResult<List<IPaymentCoupon>>.Failed(e);
            }
        }
        public async Task<PaymentGatewayResult<IPaymentRefund>> RefundChargeAsync(string chargeId,long? amount)
        {
            try
            {
                var refund = await _refundService.CreateAsync(new RefundCreateOptions
                {
                    Amount = amount/100,
                    ChargeId = chargeId,
                });
                return PaymentGatewayResult<IPaymentRefund>.Success(_mapper.Map<IPaymentRefund>(refund));

            }
            catch (StripeException e)
            {
                return PaymentGatewayResult<IPaymentRefund>.Failed(e);
            }

        }
        public async Task<PaymentGatewayResult<IPaymentCoupon>> CreateCouponAsync(CreateCouponViewModel model)
        {
            try
            {
                var options = new CouponCreateOptions();

                if (model.Duration != "repeating")
                {
                    model.DurationInMonths = null;
                }

                model.Currency = "usd";
                model.MaxRedemptions = model.MaxRedemptions > 0 ? model.MaxRedemptions : null;
                model.PercentOff = model.PercentOff > 0 ? model.PercentOff : null;
                model.AmountOff = model.AmountOff > 0 ? model.AmountOff * 100 : null;

                options = _mapper.Map<CouponCreateOptions>(model);

                var coupon = await _couponService.CreateAsync(options);
                if (coupon.PercentOff != null)
                {

                    if (coupon.Duration == "once")
                    {
                        coupon.Object = coupon.PercentOff + "% off " + coupon.Duration;
                    }
                    else if (coupon.Duration == "forever")
                    { coupon.Object = coupon.PercentOff + "% off " + coupon.Duration; }

                    else if (coupon.Duration == "repeating")
                    {
                        if (coupon.DurationInMonths == 1) coupon.Object = coupon.PercentOff + "% off every Year for 1 month";
                        else coupon.Object = coupon.PercentOff + "% off every Year for " + coupon.DurationInMonths + " months";
                    }
                }

                if (coupon.AmountOff != null)
                {
                    if (coupon.Duration == "once")
                    {
                        coupon.Object = "$" + coupon.AmountOff + " off once";
                    }

                    else if (coupon.Duration == "forever")
                    { coupon.Object = "$" + coupon.AmountOff + " off " + coupon.Duration; }

                    else if (coupon.Duration == "repeating")
                    {
                        if (coupon.DurationInMonths == 1) coupon.Object = coupon.Currency.ToUpper() + coupon.AmountOff + " off every Year for 1 month";
                        else coupon.Object = "$" + coupon.AmountOff + " off every Year for " + coupon.DurationInMonths + " months";
                    }
                }
                return PaymentGatewayResult<IPaymentCoupon>.Success(_mapper.Map<IPaymentCoupon>(coupon));
            }
            catch (StripeException e)
            {
                return PaymentGatewayResult<IPaymentCoupon>.Failed(e);
            }
        }
        public async Task<PaymentGatewayResult<IPaymentCoupon>> GetCouponByIdAsync(string id)
        {
            try
            {
                var coupon = await _couponService.GetAsync(id);
                if (coupon.PercentOff != null)
                {

                    if (coupon.Duration == "once")
                    {
                        if (coupon.DurationInMonths == 1) coupon.Object = coupon.PercentOff + "% off " + coupon.Duration + " for " + coupon.DurationInMonths + " month";
                        else coupon.Object = coupon.PercentOff + "% off " + coupon.Duration + " for " + coupon.DurationInMonths + " months";
                    }
                    else if (coupon.Duration == "forever")
                    { coupon.Object = coupon.PercentOff + "% off " + coupon.Duration; }

                    else if (coupon.Duration == "repeating")
                    {
                        if (coupon.DurationInMonths == 1) coupon.Object = coupon.PercentOff + "% off every Year for 1 month";
                        else coupon.Object = coupon.PercentOff + "% off every Year for " + coupon.DurationInMonths + " months";
                    }

                }

                if (coupon.AmountOff != null)
                {
                    coupon.AmountOff = coupon.AmountOff / 100;
                    if (coupon.Duration == "once")
                    {
                        coupon.Object = "$" + coupon.AmountOff + " off once";
                    }

                    else if (coupon.Duration == "forever")
                    { coupon.Object = "$" + coupon.AmountOff + " off " + coupon.Duration; }

                    else if (coupon.Duration == "repeating")
                    {
                        if (coupon.DurationInMonths == 1) coupon.Object = coupon.Currency.ToUpper() + coupon.AmountOff + " off every Year for 1 month";
                        else coupon.Object = "$" + coupon.AmountOff + " off every Year for " + coupon.DurationInMonths + " months";
                    }
                }
                return PaymentGatewayResult<IPaymentCoupon>.Success(_mapper.Map<IPaymentCoupon>(coupon));
            }
            catch (StripeException e)
            {
                return PaymentGatewayResult<IPaymentCoupon>.Failed(e);
            }
        }
        public async Task<PaymentGatewayResult<IPaymentCoupon>> UpdateCouponAsync(string id, CouponUpdateOptions options)
        {
            try
            {
                var coupon = await _couponService.GetAsync(id);
                coupon = await _couponService.UpdateAsync(coupon.Id, options);
                return PaymentGatewayResult<IPaymentCoupon>.Success(_mapper.Map<IPaymentCoupon>(coupon));
            }
            catch (StripeException e)
            {
                return PaymentGatewayResult<IPaymentCoupon>.Failed(e);
            }
        }
        public async Task<PaymentGatewayResult<IPaymentCoupon>> DeleteCouponAsync(string id)
        {
            try
            {
                var coupon = await _couponService.GetAsync(id);
                if (coupon == null) throw new Exception("Coupon not found");
                coupon = await _couponService.DeleteAsync(id);
                return PaymentGatewayResult<IPaymentCoupon>.Success(_mapper.Map<IPaymentCoupon>(coupon));
            }
            catch (StripeException e)
            {
                return PaymentGatewayResult<IPaymentCoupon>.Failed(e);
            }
        }
        public async Task<IPaymentCustomer> SetDefaultCardAsync(string paymentCustomerId, string paymentCardId)
        {
            var customer = await _customerSer.UpdateAsync(paymentCustomerId, new CustomerUpdateOptions
            {
                DefaultSource = paymentCardId
            });
            return _mapper.Map<IPaymentCustomer>(customer);
        }

        public async Task<PaymentGatewayResult<IPaymentCard>> GetCardAsync(string paymentCustomerId, string paymentCardId)
        {
            try
            {
                var card = await _cardService.GetAsync(paymentCustomerId, paymentCardId);
                return PaymentGatewayResult<IPaymentCard>.Success(_mapper.Map<IPaymentCard>(card));
            }
            catch (StripeException e)
            {
                return PaymentGatewayResult<IPaymentCard>.Failed(e);
            }
        }
        public async Task<PaymentGatewayResult<IPaymentCard>> CreateCardAsync(string paymentCustomerId, string paymentCardToken)
        {
            try
            {
                var card = await _cardService.CreateAsync(paymentCustomerId, new CardCreateOptions
                {
                    Source = paymentCardToken,
                });
                return PaymentGatewayResult<IPaymentCard>.Success(_mapper.Map<IPaymentCard>(card));
            }
            catch (StripeException e)
            {
                return PaymentGatewayResult<IPaymentCard>.Failed(e);
            }
        }
        public async Task<PaymentGatewayResult<IPaymentCard>> UpdateCardAsync(string paymentCustomerId, IPaymentCard card)
        {

            if (string.IsNullOrEmpty(card.Id))
                return PaymentGatewayResult<IPaymentCard>.Failed(HttpStatusCode.NotFound, "Card ID not Found");

            try
            {
                var result = await _cardService.UpdateAsync(paymentCustomerId, card.Id, _mapper.Map<CardUpdateOptions>(card));
                return PaymentGatewayResult<IPaymentCard>.Success(_mapper.Map<IPaymentCard>(result));
            }
            catch (StripeException e)
            {
                return PaymentGatewayResult<IPaymentCard>.Failed(e);
            }
        }

        public async Task<PaymentGatewayResult<IPaymentCard>> DeleteCardAsync(string paymentCustomerId, string paymentCardId)
        {
            try
            {
                var result = await _cardService.DeleteAsync(paymentCustomerId, paymentCardId);
                return PaymentGatewayResult<IPaymentCard>.Success(_mapper.Map<IPaymentCard>(result));
            }
            catch (StripeException e)
            {
                return PaymentGatewayResult<IPaymentCard>.Failed(e);
            }
        }
        public async Task<PaymentGatewayResult<IPaymentCustomer>> CreateCustomerAsync(string email, string paymentCardToken)
        {
            try
            {
                var paymentCustomer = await _customerSer.CreateAsync(new CustomerCreateOptions
                {
                    Email = email,
                    Source = paymentCardToken,

                });
                return PaymentGatewayResult<IPaymentCustomer>.Success(_mapper.Map<IPaymentCustomer>(paymentCustomer));
            }
            catch (StripeException e)
            {
                return PaymentGatewayResult<IPaymentCustomer>.Failed(e);
            }

        }

        public async Task<PaymentGatewayResult<IPaymentSubscription>> CreateSubscriptionAsync(string planId, string customerId, ChargeType type, string? coupon)
        {
            try
            {
                var options = new SubscriptionCreateOptions
                {
                    CustomerId = customerId,
                    Items = new List<SubscriptionItemOption> {
                            new SubscriptionItemOption {
                                PlanId = planId
                            }
                        },
                    CollectionMethod = _chargeTypes[(int)type],
                    CouponId = coupon
                };
                if ((int)type == 1) options.DaysUntilDue = _appSettings.Stripe.InvoiceDueDays;

                var subscription = await _subscriptionService.CreateAsync(options);
                return PaymentGatewayResult<IPaymentSubscription>.Success(_mapper.Map<IPaymentSubscription>(subscription));
            }
            catch (StripeException e)
            {
                return PaymentGatewayResult<IPaymentSubscription>.Failed(e);
            }
        }

        public async Task<PaymentGatewayResult<IPaymentSubscription>> UpdateSubscriptionAsync(string planId, ChargeType type,
            string subscriptionId, string? coupon)
        {
            try
            {
                var subscription = await _subscriptionService.GetAsync(subscriptionId);
                if (subscription == null)
                    throw new StripeException("Subscription Not Found") { HttpStatusCode = HttpStatusCode.NotFound };
                var opt = new SubscriptionUpdateOptions
                {
                    CancelAtPeriodEnd = false,
                    Items = new List<SubscriptionItemUpdateOption> {
                        new SubscriptionItemUpdateOption {
                            Id = subscription.Items.Data.FirstOrDefault()?.Id,
                            PlanId = planId
                        },
                    },
                    CouponId = coupon,
                    Prorate = true,
                    ProrationDate = DateTime.UtcNow,
                    CollectionMethod = _chargeTypes[(int)type],

                };
                if (type == ChargeType.Manual) opt.DaysUntilDue = _appSettings.Stripe.InvoiceDueDays;

                subscription = await _subscriptionService.UpdateAsync(subscription.Id, opt);
                return PaymentGatewayResult<IPaymentSubscription>.Success(_mapper.Map<IPaymentSubscription>(subscription));
            }
            catch (StripeException e)
            {
                return PaymentGatewayResult<IPaymentSubscription>.Failed(e);
            }
        }
        public async Task<PaymentGatewayResult<IPaymentInvoice>> CreateInvoiceManualAsync(long amount, string customerId)
        {
            try
            {

                var invoiceItem = await _invoiceItemService.CreateAsync(new InvoiceItemCreateOptions
                {
                    Amount = amount * 100,
                    Currency = "usd",
                    CustomerId = customerId,
                });

                var invoiceOptions = new InvoiceCreateOptions
                {
                    CustomerId = customerId,
                    AutoAdvance = false, // auto-finalize this draft after ~1 hour
                    CollectionMethod = "send_invoice",
                    DaysUntilDue = _appSettings.Stripe.InvoiceDueDays,
                    Footer = $"{Extensions.ApplicationName} Invoices"
                };
                var invoice = await _invoiceService.CreateAsync(invoiceOptions);
                return PaymentGatewayResult<IPaymentInvoice>.Success(_mapper.Map<IPaymentInvoice>(invoice));
            }
            catch (StripeException e)
            {
                return PaymentGatewayResult<IPaymentInvoice>.Failed(e);
            }

        }
        public async Task<PaymentGatewayResult<IPaymentSubscription>> CancelSubscriptionAsync(string subscriptionId, bool? invoiceNow, bool? prorate)
        {
            try
            {

                var subscription = await _subscriptionService.CancelAsync(subscriptionId,
                    new SubscriptionCancelOptions()
                    {
                        InvoiceNow = invoiceNow,
                        Prorate = prorate,
                    });
                return PaymentGatewayResult<IPaymentSubscription>.Success(_mapper.Map<IPaymentSubscription>(subscription));
            }
            catch (StripeException e)
            {
                return PaymentGatewayResult<IPaymentSubscription>.Failed(e);
            }
        }
        public async Task<PaymentGatewayResult<List<IPaymentSubscription>>> GetAllSubscriptionsAsync()
        {
            try
            {
                var subscriptions = await _subscriptionService.ListAsync(new SubscriptionListOptions
                {

                });

                return PaymentGatewayResult<List<IPaymentSubscription>>.Success(_mapper.Map<List<IPaymentSubscription>>(subscriptions));
            }
            catch (StripeException e)
            {
                return PaymentGatewayResult<List<IPaymentSubscription>>.Failed(e);
            }
        }
        public async Task<PaymentGatewayResult<IPaymentSubscription>> GetSubscriptionByIdAsync(string id)
        {
            try
            {
                var subscription = await _subscriptionService.GetAsync(id);
                return PaymentGatewayResult<IPaymentSubscription>.Success(_mapper.Map<IPaymentSubscription>(subscription));
            }
            catch (StripeException e)
            {
                return PaymentGatewayResult<IPaymentSubscription>.Failed(e);
            }
        }
        public async Task<PaymentGatewayResult<IPaymentSubscription>> UpdateSubscriptionAsync(string id, SubscriptionUpdateOptions options)
        {
            try
            {
                var subscription = await _subscriptionService.GetAsync(id);
                subscription = await _subscriptionService.UpdateAsync(subscription.Id, options);
                return PaymentGatewayResult<IPaymentSubscription>.Success(_mapper.Map<IPaymentSubscription>(subscription));
            }
            catch (StripeException e)
            {
                return PaymentGatewayResult<IPaymentSubscription>.Failed(e);
            }
        }
        public async Task<PaymentGatewayResult<IPaymentInvoice>> GetLatestInvoiceBySubscription(string subscriptionId, string customerId)
        {

            try
            {
                var prorationDate = DateTimeOffset.UtcNow;
                var options = new InvoiceListOptions
                {
                    CustomerId = customerId,
                    SubscriptionId = subscriptionId,
                };
                var invoices = await _invoiceService.ListAsync(options);
                var inv = invoices.Data.OrderByDescending(x => x.Created);
                return PaymentGatewayResult<IPaymentInvoice>.Success(
                    _mapper.Map<IPaymentInvoice>(inv.FirstOrDefault()));
            }
            catch (StripeException e)
            {
                return PaymentGatewayResult<IPaymentInvoice>.Failed(e);
            }
           
        }
        public async Task<long> CalculateProrate(string subscriptionId, string customerId)
        {
            var prorationDate = DateTimeOffset.UtcNow;
            var options = new UpcomingInvoiceOptions
            {
                CustomerId = customerId,
                SubscriptionId = subscriptionId,
                SubscriptionProrate = true,
                SubscriptionCancelNow = true,
                SubscriptionProrationDate = prorationDate.UtcDateTime
            };
            var invoice = await _invoiceService.UpcomingAsync(options);

            // Calculate the proration cost:
            long cost = 0;
            foreach (var invoiceLineItem in invoice.Lines)
            {
                cost += invoiceLineItem.Amount;
                //if (invoiceLineItem.Period.Start == null) continue;
                //var periodStart = new DateTimeOffset(invoiceLineItem.Period.Start.Value);

                //if (periodStart.ToUnixTimeSeconds() == prorationDate.ToUnixTimeSeconds())
                //{
                //    cost += invoiceLineItem.Amount;
                //}
            }

            return cost;
        }
        public async Task<long> CalculateProrate(string invoiceId)
        {
            var invoice = await GetSubscriptionByInvoiceAsync(invoiceId);
            var amount = await CalculateProrate(invoice.SubscriptionId, invoice.CustomerId);
            return amount;
        }
        //public async Task<PaymentGatewayResult<IPaymentSubscription>> CancelSubscriptionAsync(string id)
        //{
        //    try
        //    {
        //        var subscription = await _subscriptionService.GetAsync(id);
        //        subscription =await _subscriptionService.CancelAsync(subscription.EventId, null);
        //        return PaymentGatewayResult<IPaymentSubscription>.Success(_mapper.Map<IPaymentSubscription>(subscription));
        //    }
        //    catch (StripeException e)
        //    {
        //        return PaymentGatewayResult<IPaymentSubscription>.Failed(e);
        //    }
        //}
    }
}