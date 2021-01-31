using Microsoft.Extensions.DependencyInjection;

namespace Payment.Api.Core
{
    public static partial class Extensions
    {
        public static IServiceCollection AddCustomCorsPolicy(this IServiceCollection services, string MyAllowSpecificOrigins)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4300", "http://localhost:4200")
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .SetIsOriginAllowed(host => true)
                            .AllowAnyMethod();
                    });
            });
            return services;
        }
    }
}