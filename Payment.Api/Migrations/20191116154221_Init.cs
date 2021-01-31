using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Payment.Api.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coupon",
                columns: table => new
                {
                    CouponID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PaymentGatewayCouponID = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    CouponDetail = table.Column<string>(nullable: true),
                    CouponName = table.Column<string>(nullable: true),
                    CouponValidTill = table.Column<DateTime>(nullable: true),
                    Duration = table.Column<string>(nullable: true),
                    DurationInMonths = table.Column<long>(nullable: true),
                    MaxRedemptions = table.Column<int>(nullable: true),
                    TimesRedeemed = table.Column<int>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    AmountOff = table.Column<long>(nullable: true),
                    PercentOff = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    Valid = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupon", x => x.CouponID);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerID = table.Column<long>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Pic = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PaymentGatewayCustomerID = table.Column<string>(nullable: true),
                    PaymentGatewayDefaultCardID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "FeatureType",
                columns: table => new
                {
                    FeatureTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FeatureTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureType", x => x.FeatureTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PaymentGatewayProductID = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductID);
                });

            migrationBuilder.CreateTable(
                name: "Card",
                columns: table => new
                {
                    CardID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PaymentGatewayCardID = table.Column<string>(nullable: true),
                    CustomerID = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Brand = table.Column<string>(nullable: true),
                    Last4 = table.Column<string>(nullable: true),
                    ExpMonth = table.Column<long>(nullable: false),
                    ExpYear = table.Column<long>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false, defaultValue: false),
                    Created = table.Column<DateTime>(nullable: false),
                    AddressCity = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    AddressZip = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Card", x => x.CardID);
                    table.ForeignKey(
                        name: "FK_Card_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feature",
                columns: table => new
                {
                    FeatureID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>(nullable: true),
                    DataType = table.Column<string>(nullable: true),
                    ShowInSummery = table.Column<bool>(nullable: false),
                    TypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feature", x => x.FeatureID);
                    table.ForeignKey(
                        name: "FK_Feature_FeatureType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "FeatureType",
                        principalColumn: "FeatureTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Package",
                columns: table => new
                {
                    PackageID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductID = table.Column<int>(nullable: false),
                    PaymentGatewayPackageID = table.Column<string>(nullable: true),
                    PackageName = table.Column<string>(nullable: true),
                    Amount = table.Column<long>(nullable: false),
                    Interval = table.Column<string>(nullable: true),
                    IntervalCount = table.Column<long>(nullable: false),
                    UsageType = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsSubscribed = table.Column<bool>(nullable: false),
                    IsPublished = table.Column<bool>(nullable: false),
                    Features = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Package", x => x.PackageID);
                    table.ForeignKey(
                        name: "FK_Package_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    SubscriptionID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PaymentGatewaySubscriptionID = table.Column<string>(nullable: true),
                    PackageID = table.Column<int>(nullable: false),
                    CustomerID = table.Column<long>(nullable: false),
                    Usage = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    ChargeType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.SubscriptionID);
                    table.ForeignKey(
                        name: "FK_Subscription_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscription_Package_PackageID",
                        column: x => x.PackageID,
                        principalTable: "Package",
                        principalColumn: "PackageID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prorate",
                columns: table => new
                {
                    ProrateID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SubscriptionID = table.Column<long>(nullable: false),
                    PaymentGatewayInvoiceID = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<long>(nullable: false),
                    Refunded = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prorate", x => x.ProrateID);
                    table.ForeignKey(
                        name: "FK_Prorate_Subscription_SubscriptionID",
                        column: x => x.SubscriptionID,
                        principalTable: "Subscription",
                        principalColumn: "SubscriptionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Card_CustomerID",
                table: "Card",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Feature_Key",
                table: "Feature",
                column: "Key",
                unique: true,
                filter: "[Key] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Feature_TypeId",
                table: "Feature",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureType_FeatureTypeName",
                table: "FeatureType",
                column: "FeatureTypeName",
                unique: true,
                filter: "[FeatureTypeName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Package_ProductID",
                table: "Package",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Prorate_SubscriptionID",
                table: "Prorate",
                column: "SubscriptionID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_CustomerID",
                table: "Subscription",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_PackageID",
                table: "Subscription",
                column: "PackageID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Card");

            migrationBuilder.DropTable(
                name: "Coupon");

            migrationBuilder.DropTable(
                name: "Feature");

            migrationBuilder.DropTable(
                name: "Prorate");

            migrationBuilder.DropTable(
                name: "FeatureType");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Package");

            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
