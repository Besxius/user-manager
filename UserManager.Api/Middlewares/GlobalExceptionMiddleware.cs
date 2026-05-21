using FluentValidation;
using System.Net;
using System.Text.Json;

namespace UserManager.Api.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                error = exception.Message,
                details = string.Empty
            };

            if (exception is ValidationException validationException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errors = validationException.Errors.Select(e => e.ErrorMessage);
                response = new { error = "Validation Failed", details = string.Join("; ", errors) };
            }

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
