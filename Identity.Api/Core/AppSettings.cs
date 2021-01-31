using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Api.Core
{
    public static partial class Extensions
    {
        public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration Configuration)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            return services;
        }
    }
    public class Authentication
    {
        public string Scope { get; set; }
        public string Secret { get; set; }
    }
    public class AppSettings
    {
        public string AdminMail { get; set; }
        public string ApiUrl { get; set; }
        public string ApplicationName { get; set; }
        public Authentication Authentication { get; set; }
        public EmailAuth Gmail { get; set; }
        public EmailAuth Outlook { get; set; }
        public FacebookAuth Facebook { get; set; }
    }

    public class EmailAuth
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ClientSecret { get; set; }
        public string ClientId { get; set; }
    }
    public class FacebookAuth
    {
        public string ClientSecret { get; set; }
        public string ClientId { get; set; }
    }

}