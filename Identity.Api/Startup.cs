using System;
using System.Linq;
using EventBus.Common.Abstractions;
using Identity.Api.Core;
using Identity.Api.DataAccess;
using Identity.Api.Events;
using Identity.Api.Services;
using Microservices.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
namespace Identity.Api
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
            Extensions.AdminId = 2;
            if (HostingEnv.IsDevelopment())
            {
                Console.WriteLine("Debug");
                Extensions.AdminUrl = "http://localhost:4200";
                Extensions.ApiUrl = "http://localhost:5000";
            }
            else
            {
                Console.WriteLine("No-Debug");
                Extensions.AdminUrl = "https://sharedframework.azurewebsites.net";
                Extensions.ApiUrl = $"{ Extensions.AdminUrl }/api/upload";
            }

            services

                .AddCustomDbContext(HostingEnv, Configuration)
                .AddCustomCorsPolicy(MyAllowSpecificOrigins)
                .AddCustomAutoMapper()
                .AddCustomSwaggerGen()
                .AddCustomIdentity()
                .AddSettings(Configuration)

                // Authentication & Permission Authorization
                .AddCustomAuthentication(Configuration)
                .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>()
                .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()

                // for User account management
                .AddScoped<IAccountManager, AccountManager>()

                // Email Services
                .AddScoped<IEmailSender, EmailSender>()
                .AddSingleton<ITokenGenerator, TokenGenerator>()
                .AddCustomEventBus(Configuration)
                ;

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            return services.AddAutofacServiceProvider();
        }
        private void ConfigureEnvironmentVariables(IApplicationBuilder app)
        {
            var settings = app.ApplicationServices
                .GetService<IOptions<AppSettings>>()
                .Value;

            Extensions.ApiUrl = settings.ApiUrl.GetDefaultValue();
            Extensions.AdminMail = settings.AdminMail.GetDefaultValue();
            Extensions.ApplicationName = settings.ApplicationName.GetDefaultValue();
        }
        private void ConfigureEventBus(IApplicationBuilder app)
        {
            //var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            //eventBus.Subscribe<EmailS>();
            //eventBus.Subscribe<ValueChangedIntegrationEvent, ValueChangedIntegrationEventHandler>();
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

            app.UseApiExceptionMiddleware();
            // app.UseStaticFiles(); // For the wwwroot folder     
            app.UseStaticFiles(new StaticFileOptions
            {
                // FileProvider = new PhysicalFileProvider("/usr/app/uploads"),
                RequestPath = "/static",
                OnPrepareResponse = ctx => {
                    ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                    ctx.Context.Response.Headers.Append("Access-Control-Allow-Headers", "Image/svg+xml, text/plain, text/html, Origin, X-Requested-With, Content-Type, Accept, Application/x-www-form-urlencoded,Multipart/form-data");
                }

            });
            app.UseCors(MyAllowSpecificOrigins);
            app.UseCustomSwagger(env);
            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller}/{action}");
            });

            //app.UseHttpsRedirection();
            ConfigureEventBus(app);
            ConfigureEnvironmentVariables(app);
            app.UseMvc();
        }
    }
}
