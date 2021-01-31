using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventBus.Common.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Api.Core
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
