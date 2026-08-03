using Microsoft.OpenApi;
using SniffAndStay.API.SchemaFilter;
using System.Reflection;

namespace SniffAndStay.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                options.SchemaFilter<EnumSchemaFilter>();

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Sniff & Stay API",
                    Version = "v1",
                    Description = "API pentru aplicația Sniff & Stay - locații pet-friendly"
                });

                options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization"
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("bearer", document)] = []
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}
