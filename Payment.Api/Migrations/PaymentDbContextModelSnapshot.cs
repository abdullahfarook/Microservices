﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Payment.Api.DataAccess;

namespace Payment.Api.Migrations
{
    [DbContext(typeof(PaymentDbContext))]
    partial class PaymentDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Payment.Api.DataAccess.Model.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("CardID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AddressCity");

                    b.Property<string>("AddressLine1");

                    b.Property<string>("AddressLine2");

                    b.Property<string>("AddressZip");

                    b.Property<string>("Brand");

                    b.Property<string>("Country");

                    b.Property<DateTime>("Created");

                    b.Property<long>("CustomerId")
                        .HasColumnName("CustomerID");

                    b.Property<long>("ExpMonth");

                    b.Property<long>("ExpYear");

                    b.Property<bool>("IsDefault")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("Last4");

                    b.Property<string>("Name");

                    b.Property<string>("PaymentGatewayCardId")
                        .HasColumnName("PaymentGatewayCardID");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Card");
                });

            modelBuilder.Entity("Payment.Api.DataAccess.Model.Coupon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("CouponID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("AmountOff");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Currency");

                    b.Property<string>("Duration");

                    b.Property<long?>("DurationInMonths");

                    b.Property<int?>("MaxRedemptions");

                    b.Property<string>("Name")
                        .HasColumnName("CouponName");

                    b.Property<string>("Object")
                        .HasColumnName("CouponDetail");

                    b.Property<string>("PaymentGatewayCouponId")
                        .HasColumnName("PaymentGatewayCouponID");

                    b.Property<decimal?>("PercentOff")
                        .HasColumnType("decimal(18,4)");

                    b.Property<DateTime?>("RedeemBy")
                        .HasColumnName("CouponValidTill");

                    b.Property<int?>("TimesRedeemed");

                    b.Property<bool>("Valid");

                    b.HasKey("Id");

                    b.ToTable("Coupon");
                });

            modelBuilder.Entity("Payment.Api.DataAccess.Model.Customer", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnName("CustomerID");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("FullName");

                    b.Property<string>("LastName");

                    b.Property<string>("PaymentGatewayCustomerId")
                        .HasColumnName("PaymentGatewayCustomerID");

                    b.Property<string>("PaymentGatewayDefaultCardId")
                        .HasColumnName("PaymentGatewayDefaultCardID");

                    b.Property<string>("Pic");

                    b.HasKey("Id");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("Payment.Api.DataAccess.Model.Feature", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("FeatureID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DataType");

                    b.Property<string>("Key");

                    b.Property<bool>("ShowInSummery");

                    b.Property<int>("TypeId");

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique()
                        .HasFilter("[Key] IS NOT NULL");

                    b.HasIndex("TypeId");

                    b.ToTable("Feature");
                });

            modelBuilder.Entity("Payment.Api.DataAccess.Model.FeatureType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("FeatureTypeID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnName("FeatureTypeName");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[FeatureTypeName] IS NOT NULL");

                    b.ToTable("FeatureType");
                });

            modelBuilder.Entity("Payment.Api.DataAccess.Model.Package", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PackageID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("Amount");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Description");

                    b.Property<string>("Features");

                    b.Property<string>("Interval");

                    b.Property<long>("IntervalCount");

                    b.Property<bool>("IsPublished");

                    b.Property<bool>("IsSubscribed");

                    b.Property<string>("Name")
                        .HasColumnName("PackageName");

                    b.Property<string>("PaymentGatewayPackageId")
                        .HasColumnName("PaymentGatewayPackageID");

                    b.Property<int>("ProductId")
                        .HasColumnName("ProductID");

                    b.Property<DateTime>("UpdatedOn");

                    b.Property<string>("UsageType");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Package");
                });

            modelBuilder.Entity("Payment.Api.DataAccess.Model.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ProductID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("PaymentGatewayProductId")
                        .HasColumnName("PaymentGatewayProductID");

                    b.Property<string>("Type");

                    b.Property<DateTime>("UpdatedOn");

                    b.HasKey("Id");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("Payment.Api.DataAccess.Model.Prorate", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ProrateID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("Amount");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("PaymentGatewayInvoiceId")
                        .HasColumnName("PaymentGatewayInvoiceID");

                    b.Property<bool>("Refunded")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<long>("SubscriptionId")
                        .HasColumnName("SubscriptionID");

                    b.HasKey("Id");

                    b.HasIndex("SubscriptionId")
                        .IsUnique();

                    b.ToTable("Prorate");
                });

            modelBuilder.Entity("Payment.Api.DataAccess.Model.Subscription", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("SubscriptionID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChargeType");

                    b.Property<long>("CustomerId")
                        .HasColumnName("CustomerID");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<int>("PackageId")
                        .HasColumnName("PackageID");

                    b.Property<string>("PaymentGatewaySubscriptionId")
                        .HasColumnName("PaymentGatewaySubscriptionID");

                    b.Property<string>("Usage");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("PackageId");

                    b.ToTable("Subscription");
                });

            modelBuilder.Entity("Payment.Api.DataAccess.Model.Card", b =>
                {
                    b.HasOne("Payment.Api.DataAccess.Model.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Payment.Api.DataAccess.Model.Feature", b =>
                {
                    b.HasOne("Payment.Api.DataAccess.Model.FeatureType", "Type")
                        .WithMany("Features")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Payment.Api.DataAccess.Model.Package", b =>
                {
                    b.HasOne("Payment.Api.DataAccess.Model.Product", "Product")
                        .WithMany("Packages")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Payment.Api.DataAccess.Model.Prorate", b =>
                {
                    b.HasOne("Payment.Api.DataAccess.Model.Subscription", "Subscription")
                        .WithOne("Prorate")
                        .HasForeignKey("Payment.Api.DataAccess.Model.Prorate", "SubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Payment.Api.DataAccess.Model.Subscription", b =>
                {
                    b.HasOne("Payment.Api.DataAccess.Model.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Payment.Api.DataAccess.Model.Package", "Package")
                        .WithMany()
                        .HasForeignKey("PackageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}