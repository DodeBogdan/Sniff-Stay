using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SniffAndStay.Application.Exceptions;

namespace SniffAndStay.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (InvalidUserException ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(
                    new
                    {
#if DEBUG
                        message = ex.Message
#else
                        message = "Invalid email or password"
#endif
                    });

            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(
                    new
                    {
                        message = ex.Message
                    });
            }
            catch (ValidationException ex)
            {
                var modelState = new ModelStateDictionary();
                foreach (var error in ex.Errors)
                    modelState.AddModelError(error.PropertyName, error.ErrorMessage);

                var problemDetails = new ValidationProblemDetails(modelState)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation failed"
                };

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = 500;

                await context.Response.WriteAsJsonAsync(
                    new
                    {
#if DEBUG
                        message = ex.Message
#else
                        message = "An unexpected error occurred. Please try again later."
#endif
                    });
            }
        }
    }
}