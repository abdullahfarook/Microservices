using System;
using AutoMapper;
using Identity.Api.DataAccess.Model;
using Identity.Api.ViewModel;
using Microservices.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Api.Core
{
    public static partial class Extensions
    {
        public static IServiceCollection AddCustomAutoMapper(this IServiceCollection services)
        {
            // Configuring AutoMapper
            services.AddAutoMapper(m => {
                m.CreateMissingTypeMaps = true;
                m.EnableNullPropagationForQueryMapping = true;
                m.ValidateInlineMaps = false;
                m.AddProfile<IdentityProfile>();
            });
            return services;
        }
    }
    public class IdentityProfile : Profile
    {
        public IdentityProfile()
        {
            CreateMap<User, UserViewModel>()
                .IgnoreNull();
            CreateMap<UserViewModel, User>()
                .ForMember(d => d.Roles, op => op.Ignore())
                .IgnoreNullAndDefault();

            CreateMap<RegisterViewModel, User>()
                .ForMember(d => d.Id, op => op.Ignore())
                .AfterMap((s, d) =>
                {
                    d.CreatedOn = DateTime.UtcNow;
                    d.LatestUpdatedOn = DateTime.UtcNow;
                    d.IsBlocked = false;
                });
        }
    }
}
