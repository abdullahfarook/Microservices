using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Payment.Api.Core
{
    public static partial class Extensions
    {
        public static IServiceProvider AddAutofacServiceProvider(this IServiceCollection services)
        {
            var container = new ContainerBuilder();
            container.Populate(services);
            container.RegisterEventHandlers();
            return new AutofacServiceProvider(container.Build());
        }
    }
}
