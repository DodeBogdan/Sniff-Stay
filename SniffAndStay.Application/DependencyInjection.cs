using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SniffAndStay.Application.Authentication.Commands.BackdoorUser;
using SniffAndStay.Application.Common.Behaviors;
using System.Reflection;

namespace SniffAndStay.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);

                cfg.AddOpenBehavior(
                    typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(assembly);

            services.Configure<DefaultUserConfiguration>(configuration.GetSection(DefaultUserConfiguration.SectionName));

            return services;
        }
    }
}
