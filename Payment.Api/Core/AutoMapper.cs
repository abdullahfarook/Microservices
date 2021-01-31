using System;
using System.Collections.Generic;
using AutoMapper;
using Microservices.Core;
using Microsoft.Extensions.DependencyInjection;
using Payment.Api.DataAccess.Model;
using Payment.Api.Events;
using Payment.Api.Services;
using Payment.Api.ViewModel;
using Stripe;
using Card = Payment.Api.DataAccess.Model.Card;
using Product = Payment.Api.DataAccess.Model.Product;
using Coupon = Payment.Api.DataAccess.Model.Coupon;
using Customer = Payment.Api.DataAccess.Model.Customer;

namespace Payment.Api.Core
{
    public static partial class Extensions
    {
        public static IServiceCollection AddCustomAutoMapper(this IServiceCollection services)
        {
            // Configuring AutoMapper
            services.AddAutoMapper(m => {
                m.CreateMissingTypeMaps = true;
                m.EnableNullPropagationForQueryMapping = true;
                m.ValidateInlineMaps = false;
                m.AddProfile<StripeProfile>();
            });
            return services;
        }
    }
    public class StripeProfile : Profile
    {
        public StripeProfile()
        {
            CreateMap<IPaymentPackage, Package>()
                .ForMember(d => d.Id, op => op.Ignore())
                .AfterMap((s, d) => { d.PaymentGatewayPackageId = s.Id; })
                .IgnoreNull();
            CreateMap<Package, IPaymentPackage>()
                .ForMember(d => d.Id, op => op.Ignore());
            CreateMap<IPaymentProduct, Product>()
                // .ForMember(d => d.EventId, op => op.Ignore())
                .IgnoreNullAndDefault()
                .AfterMap((s, d) =>
                {
                    d.CreatedOn = DateTime.UtcNow;
                    d.UpdatedOn = DateTime.UtcNow;
                    d.PaymentGatewayProductId = s.Id;
                });
            CreateMap<IPaymentProduct, ProductCreateOptions>()
                .AfterMap((s, d) =>
                {
                    d.Type = "service";
                    d.Metadata = new Dictionary<string, string>
                    {
                        {"Description", s.Description}
                    };
                });

            CreateMap<IPaymentProduct, ProductUpdateOptions>()
                .AfterMap((s, d) =>
                {
                    d.Description = null;
                    d.Metadata = new Dictionary<string, string>
                    {
                        {"Description", s.Description}
                    };
                });

            CreateMap<Product, IPaymentProduct>()
                .ForMember(d => d.Id, op => op.Ignore())
                .AfterMap((s, d) =>
                {
                    d.Description = null;
                    d.Id = s.PaymentGatewayProductId;
                });

            CreateMap<IPaymentPackage, PlanCreateOptions>()
                .AfterMap((s, d) => { d.Nickname = s.Name; });

            CreateMap<Plan, IPaymentPackage>()
                .AfterMap((s, d) => { d.Name = s.Nickname; });

            CreateMap<Stripe.Card, IPaymentCard>();
            CreateMap<Stripe.Invoice, IPaymentInvoice>();
            CreateMap<IPaymentCard, CardUpdateOptions>()
                .IgnoreNull();
            CreateMap<CardViewModel, IPaymentCard>()
                .IgnoreNullAndDefault();
            CreateMap<IPaymentCard, Card>()
                .ForMember(d => d.Id, op => op.Ignore())
                .AfterMap((s, d) => { d.PaymentGatewayCardId = s.Id; })
                .IgnoreNullAndDefault();

            CreateMap<IPaymentCoupon, Coupon>()
                .ForMember(d => d.Id, op => op.Ignore())
                .AfterMap((s, d) => { d.PaymentGatewayCouponId = s.Id; })
                .IgnoreNull();

            CreateMap<CreateCouponViewModel, CouponCreateOptions>()
                .ForMember(d => d.Id, op => op.Ignore())
                .IgnoreNullAndDefault();

            CreateMap<UpdateUserIntegrationEvent, Customer>()
                .IgnoreNullAndDefault();
        }
    }
}