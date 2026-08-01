using SniffAndStay.Application.Exceptions;

namespace SniffAndStay.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            //TODO: Find a way to catch the InvalidUserException and return a 404 status code with a
            //better message than "Invalid email or password..." and logg the exception in the database.
            //catch (InvalidUserException ex)
            //{
            //    //log in db ex.message;
            //    context.Response.StatusCode = 404;
            //    await context.Response.WriteAsJsonAsync(
            //        new
            //        {
            //            message = "Invalid email or password..."
            //        });
            //}
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(
                    new
                    {
                        message = ex.Message
                    });
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;

                await context.Response.WriteAsJsonAsync(
                    new
                    {
                        message = ex.Message
                    });
            }
        }
    }
}
