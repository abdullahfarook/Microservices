using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventBus.Common;
using EventBus.Common.Abstractions;
using EventBus.RabbitMQ;
using Microservices.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Payment.Api.Core;
using Payment.Api.Events;
using Payment.Api.Services;
using Stripe;

namespace Payment.Api
{
    public class Startup
    {

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnv)
        {
            Configuration = configuration;
            HostingEnv = hostingEnv;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnv { get; }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            Extensions.ApplicationName = Configuration["ApplicationName"];

            services
                .AddCustomEventBus(Configuration)
                .AddCustomDbContext(HostingEnv, Configuration)
                .AddCustomCorsPolicy(MyAllowSpecificOrigins)
                .AddCustomAutoMapper()
                .AddCustomSwaggerGen()
                .AddSettings(Configuration)


                .AddScoped<IEmailSender, EmailSender>()

                // Stripe Services
                .AddScoped<ProductService>()
                .AddScoped<PlanService>()
                .AddScoped<CustomerService>()
                .AddScoped<CardService>()
                .AddScoped<InvoiceService>()
                .AddScoped<CouponService>()
                .AddScoped<RefundService>()
                .AddScoped<SubscriptionService>()
                .AddScoped<InvoiceItemService>()
                .AddScoped<IPaymentGateway, StripePaymentGateway>()


                // Authorization & Authentication
                .AddCustomAuthentication(Configuration)
                .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>()
                .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();



            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            return services.AddAutofacServiceProvider();
        }

        private void ConfigureEnvironmentVariables(IApplicationBuilder app)
        {
            var settings = app.ApplicationServices
                .GetService<IOptions<AppSettings>>()
                .Value;

            Extensions.ApiUrl = settings.ApiUrl;
            Extensions.AdminMail = settings.AdminMail;
            Extensions.ApplicationName = settings.ApplicationName;
        }
        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<UpdateUserIntegrationEvent, UpdateUserIntegrationEventHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthentication();
            app.UseCustomSwagger(env);
            app.UseStaticFiles();
            //app.UseHttpsRedirection();

            ConfigureEventBus(app);
            ConfigureEnvironmentVariables(app);

            app.UseMvc();
        }
    }
}
