using OsirisOnlineBetting.Models.Responses;
using System.Text.Json;

namespace OsirisOnlineBetting.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred: {ex.Message}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                KeyNotFoundException => StatusCodes.Status404NotFound,
                ArgumentException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            var errorResponse = new ErrorResponse
            {
                StatusCode = context.Response.StatusCode,
                Message = GetCustomMessage(context.Response.StatusCode),
                Details = exception.Message 
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }

        private string GetCustomMessage(int statusCode)
        {
            return statusCode switch
            {
                StatusCodes.Status404NotFound => "The resource you are looking for was not found.",
                StatusCodes.Status400BadRequest => "Invalid request, please check the input data.",
                _ => "An unexpected error occurred. Please try again later."
            };
        }

    }
}
