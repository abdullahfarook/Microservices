using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Identity.Api.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Identity.Api.Core
{
    public static partial class Extensions {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IHostingEnvironment hostingEnv,
            IConfiguration configuration)
        {

            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DatabaseConnection")));

            // Migrations
            if (!hostingEnv.IsDevelopment()) 
                ApplyMigrations(services.BuildServiceProvider().GetService<IdentityDbContext>());

            return services;
        }
        public static void ApplyMigrations(IdentityDbContext context)
        {
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
    }
}
