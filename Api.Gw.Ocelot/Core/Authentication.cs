using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Api.Gw.Ocelot.Core
{
    public static partial class Extensions
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration Configuration)
        {
            var identityUrl = Configuration.GetValue<string>("IdentityUrl");
            //var authenticationProviderKey = "IdentityApiKey";

            services.AddAuthentication()
                .AddJwtBearer(x =>
                {
                    //x.Authority = identityUrl;
                    x.RequireHttpsMetadata = false;
                    x.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidAudiences = new[] { "identity"}
                    };
                    x.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = async ctx =>
                        {
                            int i = 0;
                        },
                        OnTokenValidated = async ctx =>
                        {
                            int i = 0;
                        },

                        OnMessageReceived = async ctx =>
                        {
                            int i = 0;
                        }
                    };
                });
            return services;
        }
    }
}
