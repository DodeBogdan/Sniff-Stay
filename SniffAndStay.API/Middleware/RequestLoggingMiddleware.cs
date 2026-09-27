using SniffAndStay.Application.Interfaces.Security;
using System.Diagnostics;

namespace SniffAndStay.API.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly bool _isRequestLoggingEnabled;

        public RequestLoggingMiddleware(RequestDelegate next,
            ILoggerFactory loggerFactory,
            IConfiguration configuration)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger("RequestLogging");
            _isRequestLoggingEnabled = configuration.GetValue<bool>("RequestLoggingEnabled");
        }

        public async Task Invoke(HttpContext context, ICurrentUserService currentUserService)
        {
            if (!_isRequestLoggingEnabled)
            {
                await _next(context);
                return;
            }

            Stopwatch stopwatch = Stopwatch.StartNew();
            await _next(context);

            stopwatch.Stop();

            _logger.LogInformation(
                "{Method} {Path} responded {StatusCode} in {DurationMs}ms, User={UserId}, IP={IpAddress}, Agent={UserAgent}",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                currentUserService.UserId,
                context.Connection.RemoteIpAddress?.ToString(),
                context.Request.Headers.UserAgent.ToString());
        }
    }
}