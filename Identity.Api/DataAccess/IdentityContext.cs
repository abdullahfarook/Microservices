using System;
using Identity.Api.DataAccess.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.DataAccess
{
    public class IdentityDbContext : IdentityDbContext<User, Role, long>
    {
        public IdentityDbContext(DbContextOptions options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Override default AspNet Identity table names
            builder.Entity<User>(entity => { entity.ToTable(name: "User"); });
            builder.Entity<Role>(entity => { entity.ToTable(name: "Role"); });
            builder.Entity<IdentityUserRole<long>>(entity => { entity.ToTable("UserRoleMap"); });
            builder.Entity<IdentityUserClaim<long>>(entity => { entity.ToTable("UserClaim"); });
            builder.Entity<IdentityUserLogin<long>>(entity => { entity.ToTable("UserLogin"); });
            builder.Entity<IdentityUserToken<long>>(entity => { entity.ToTable("UserToken"); });
            builder.Entity<IdentityRoleClaim<long>>(entity => { entity.ToTable("RoleClaim"); });

            builder.Entity<User>().HasMany(u => u.UserClaims).WithOne().HasForeignKey(c => c.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<User>().HasMany(u => u.Roles).WithOne().HasForeignKey(r => r.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Role>().HasMany(r => r.RoleClaims).WithOne().HasForeignKey(c => c.RoleId).IsRequired();
            builder.Entity<Role>().HasMany(r => r.Users).WithOne().HasForeignKey(r => r.RoleId).IsRequired();


            // Seed Data
            builder.Entity<Role>()
                .HasData(
                    new Role
                    {
                        Id = 1,
                        ConcurrencyStamp = "1af537c3-f4c8-4648-b94d-f908113b6f63",
                        CreatedOn = new DateTime(2019, 9, 24, 14, 14, 24, 431, DateTimeKind.Utc),
                        UpdatedOn = new DateTime(2019, 9, 24, 14, 14, 24, 431, DateTimeKind.Utc),
                        Description = "For Client Side",
                        Name = "User",
                        NormalizedName = "USER"
                    },
                    new Role
                    {
                        Id = 2,
                        ConcurrencyStamp = "abb441ea-8bd8-48be-8c58-28258d227f85",
                        CreatedOn = new DateTime(2019, 9, 24, 14, 14, 24, 431, DateTimeKind.Utc),
                        UpdatedOn = new DateTime(2019, 9, 24, 14, 14, 24, 431, DateTimeKind.Utc),
                        Description = "For Admin Side",
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    },
                    new Role
                    {
                        Id = 3,
                        ConcurrencyStamp = "abb441ea-8bd8-48be-8c58-28258d227f93",
                        CreatedOn = new DateTime(2019, 9, 24, 14, 14, 24, 431, DateTimeKind.Utc),
                        UpdatedOn = new DateTime(2019, 9, 24, 14, 14, 24, 431, DateTimeKind.Utc),
                        Description = "For Contributor",
                        Name = "Contributor",
                        NormalizedName = "CONTRIBUTOR"
                    });

        }
    }
}
