using NLog;
using NLog.Web;
using SniffAndStay.API;
using SniffAndStay.API.Middleware;
using SniffAndStay.Application;
using SniffAndStay.Infrastructure;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddNLogConfiguration(builder.Configuration);

try
{
    builder.Services.AddApiServices();
    builder.Services.AddApplicationServices(builder.Configuration);
    builder.Services.AddInfrastructureServices(builder.Configuration);

    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);

    builder.Host.UseNLog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Sniff & Stay API v1");
        });

    }

    app.UseMiddleware<ExceptionMiddleware>();
    app.UseMiddleware<RequestLoggingMiddleware>();
    app.UseCors("AllowAngularApp");
    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}catch (Exception ex)
{
    logger.Error(ex, "Application stopped because of an exception", ex);
    throw;
}
finally
{
    LogManager.Shutdown();
}

