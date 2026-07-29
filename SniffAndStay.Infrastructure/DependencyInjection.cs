using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SniffAndStay.Application.Interfaces.Persistence;
using SniffAndStay.Application.Interfaces.Security;
using SniffAndStay.Infrastructure.Persistence;
using SniffAndStay.Infrastructure.Security;

namespace SniffAndStay.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IApplicationDbContext>(
                provider => provider.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<IPasswordHasher<object>, PasswordHasher<object>>();
            services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            services.AddScoped<IJwtService, JwtService>();


            return services;
        }
    }
}
