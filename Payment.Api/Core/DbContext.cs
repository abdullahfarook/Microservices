using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment.Api.DataAccess;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Payment.Api.Core
{
    public static partial class Extensions {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IHostingEnvironment hostingEnv,
            IConfiguration configuration)
        {

            services.AddDbContext<PaymentDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DatabaseConnection")));

            // Migrations
            if (!hostingEnv.IsDevelopment()) 
                ApplyMigrations(services.BuildServiceProvider().GetService<PaymentDbContext>());

            return services;
        }
        public static void ApplyMigrations(PaymentDbContext context)
        {
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
    }
}
