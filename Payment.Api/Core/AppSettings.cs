using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Payment.Api.Core
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

    public class StripeSettings
    {
        public string SecretKey { get; set; }
        public string PublishableKey { get; set; }
        public long InvoiceDueDays { get; set; }
    }

    public class Authentication
    {
        public string Scope { get; set; }
        public string Secret { get; set; }
    }
    public class AppSettings
    {
        public string ApiUrl { get; set; } = Extensions.ApiUrl;
        public string ApplicationName { get; set; } = Extensions.ApplicationName;
        public string AdminMail { get; set; } = Extensions.AdminMail;
        public Authentication Authentication { get; set; }
        public StripeSettings Stripe { get; set; }
    }


}