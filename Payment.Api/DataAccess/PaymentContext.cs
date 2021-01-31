using Microsoft.EntityFrameworkCore;
using Payment.Api.DataAccess.Model;

namespace Payment.Api.DataAccess
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<FeatureType> FeatureTypes { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Prorate> Prorates { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Card>()
                .Property(b => b.IsDefault)
                .HasDefaultValue(0);
            builder.Entity<Subscription>()
                .Property(b => b.IsDeleted)
                .HasDefaultValue(false);
            builder.Entity<Prorate>()
                .Property(b => b.Refunded)
                .HasDefaultValue(false);

            builder.Entity<Coupon>().Property(co => co.PercentOff).HasColumnType("decimal(18,4)");
            builder.Entity<Feature>(entity => {
                entity.HasIndex(e => e.Key).IsUnique();
            });
            builder.Entity<FeatureType>(entity => {
                entity.HasIndex(e => e.Name).IsUnique();
            });
        }
    }
}
