using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Payment.Api.Core
{
    public static partial class Extensions
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration Configuration)
        {
            var settings = Configuration
                .GetSection("AppSettings")
                .Get<AppSettings>();

            var key = Encoding.ASCII.GetBytes(settings.Authentication.Scope);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                //x.Authority = Configuration.GetValue<string>("IdentityUrl");
                x.RequireHttpsMetadata = false;
                //x.SaveToken = true;
                //x.Audience = "payment";
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateAudience = false,
                    ValidateIssuer = false,
                };
            });
            return services;
        }
    }
}
