using System;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Identity.Api.Core
{
    public static partial class Extensions
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration Configuration)
        {
            var settings = Configuration
                .GetSection("AppSettings")
                .Get<AppSettings>();
            Console.WriteLine(JsonConvert.SerializeObject(settings));
            var key = Encoding.ASCII.GetBytes(settings.Authentication.Secret);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(x => {
                    x.RequireHttpsMetadata = false;
                    //x.SaveToken = true;
                    //x.Authority = Configuration.GetValue<string>("IdentityUrl");
                    //x.Audience = "identity";
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        //ValidAudiences = new[] { "identity", "payment", "powerbi" },
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                })
                .AddGoogle(options =>
                {
                    options.CallbackPath = new PathString("/google-callback");
                    options.ClientId = settings.Gmail.ClientId;
                    options.ClientSecret = settings.Gmail.ClientSecret;
                }).AddMicrosoftAccount(microsoftOptions =>
                {
                    microsoftOptions.CallbackPath = new PathString("/signin-microsoft");
                    microsoftOptions.ClientId = settings.Outlook.ClientId;
                    microsoftOptions.ClientSecret = settings.Outlook.ClientSecret;
                }).AddFacebook(facebookOptions =>
                {
                    facebookOptions.CallbackPath = new PathString("/signin-facebook");
                    facebookOptions.AppId = settings.Facebook.ClientId;
                    facebookOptions.AppSecret = settings.Facebook.ClientSecret;
                });
            return services;
        }
    }
}
