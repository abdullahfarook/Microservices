using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Identity.Api.Core
{
    public static partial class Extensions
    {
        public static IServiceCollection AddCustomSwaggerGen(this IServiceCollection services)
        {
            var apiKeyScheme = new ApiKeyScheme
            {
                Description = "JWT Authorization Scheme",
                Name = "Authorization",
                In = "header",
                Type = "apiKey"
            };

            services.AddSwaggerGen(i =>
            {
                i.SwaggerDoc("v1", new Info { Title = "Identity Api", Version = "v1" });
                i.AddSecurityDefinition("Bearer", apiKeyScheme);
                i.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }}
                });
            });
            return services;
        }
        public static void UseCustomSwagger(this IApplicationBuilder app, IHostingEnvironment HostingEnv)
        {
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                if (HostingEnv.IsDevelopment())
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = "api";
                }
                else
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = "api";
                }
            });
        }
    }
}