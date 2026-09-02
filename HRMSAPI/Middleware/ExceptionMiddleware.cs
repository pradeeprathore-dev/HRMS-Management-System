using System.Net;
using System.Text.Json;

namespace HRMSAPI.Middleware
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
            catch (Exception ex)
            {
                context.Response.ContentType =
                    "application/json";

                context.Response.StatusCode =
                    (int)HttpStatusCode.InternalServerError;

                var response = new
                {
                    StatusCode =
                        context.Response.StatusCode,

                    Message =
                        "Internal Server Error",

                    Details =
                        ex.InnerException?.Message
                        ?? ex.Message
                };

                var json =
                    JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(json);
            }
        }
    }
}