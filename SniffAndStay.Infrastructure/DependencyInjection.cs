using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SniffAndStay.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));


            return services;
        }
    }
}
