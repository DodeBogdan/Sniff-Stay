using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Config;
using NLog.Targets;
using SniffAndStay.Application.Common.Configurations;
using SniffAndStay.Application.Interfaces.Common;
using SniffAndStay.Application.Interfaces.Persistence;
using SniffAndStay.Application.Interfaces.Security;
using SniffAndStay.Infrastructure.Common;
using SniffAndStay.Infrastructure.Configuration;
using SniffAndStay.Infrastructure.Persistence;
using SniffAndStay.Infrastructure.Security;
using System.Text;

namespace SniffAndStay.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddNLogConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConnectionStringsConfiguration>(configuration.GetSection(ConnectionStringsConfiguration.SectionName));
            var connectionStringsConfiguration = configuration.GetSection(ConnectionStringsConfiguration.SectionName).Get<ConnectionStringsConfiguration>()!;

            ConfigureNLogDatabaseTargetConnectionString("database", connectionStringsConfiguration.LoggingConnection);
        }
        private static void ConfigureNLogDatabaseTargetConnectionString(string targetName, string connectionString)
        {
            LoggingConfiguration config = LogManager.Configuration;

            if (config.FindTargetByName(targetName) is DatabaseTarget databaseTarget)
            {
                databaseTarget.ConnectionString = connectionString;
            }

            LogManager.Configuration = config;
        }

        public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConnectionStringsConfiguration>(configuration.GetSection(ConnectionStringsConfiguration.SectionName));
            var connectionStringsConfiguration = configuration.GetSection(ConnectionStringsConfiguration.SectionName).Get<ConnectionStringsConfiguration>()!;

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionStringsConfiguration.DatabaseConnection));

            services.AddScoped<IApplicationDbContext>(
                provider => provider.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<IPasswordHasher<object>, PasswordHasher<object>>();
            services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IUserService, UserService>();

            services.AddSingleton<IFilePathService, FilePathService>();
            services.Configure<FileStorageConfig>(configuration.GetSection(FileStorageConfig.SectionName));
            var fileStorageConfig = configuration.GetSection(FileStorageConfig.SectionName).Get<FileStorageConfig>()!;
            
            if (fileStorageConfig.UseLocalStorage)
            {
                services.AddSingleton<IFileStorageService, LocalFileStorageService>();
            }
            else
            {
                services.AddSingleton<IFileStorageService, CloudFileStorageService>();
            }



            // Authentication
            services.Configure<JwtConfig>(configuration.GetSection(JwtConfig.SectionName));

            var jwtConfig = configuration.GetSection(JwtConfig.SectionName).Get<JwtConfig>()!;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.MapInboundClaims = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtConfig.Issuer,
                        ValidAudience = jwtConfig.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtConfig.Key)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // Authorization
            services.AddAuthorization();
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
