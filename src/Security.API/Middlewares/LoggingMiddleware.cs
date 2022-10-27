using System.Text.Json;
using System.Text.Json.Serialization;
using N5.Challenge.Services.Security.API.Infrastructure.Exceptions;
using N5.Challenge.Services.Security.API.Responses;

namespace N5.Challenge.Services.Security.API.Middlewares
{
    public sealed class LoggingMiddleware : IMiddleware
    {
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(ILogger<LoggingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                int statusCode = GetStatusCode(ex);

                if (statusCode == 500)
                {
                    _logger.LogError(ex.ToString());
                }

                await HandleExceptionAsync(context, ex, statusCode);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex, int statusCode)
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var response = new ErrorResponse(
                GetMessage(context, ex),
                statusCode,
                GetTitle(ex),
                GetErrors(ex)
            );
            

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }

        private static string GetCustomMessage(HttpContext context)
        {
            return context.Request.Method switch
            {
                "DELETE" => "An error occurred while deleting the resource.",
                "GET" => "An error has occurred obtaining the resource.",
                "PATCH" => "An error occurred while trying to update the resource.",
                "POST" => "An error occurred while trying to create the resource.",
                "PUT" => "An error occurred while trying to update the resource.",
                _ => "An error occurred while consuming the service."
            };
        }
        

        private static string GetMessage(HttpContext context, Exception ex) =>
            ex switch
            {
                BadRequestException => ex.Message,
                NotFoundException => ex.Message,
                ValidationException => ex.Message,
                TaskCanceledException => ex.Message,
                OperationCanceledException => ex.Message,
                _ => GetCustomMessage(context)
            };

        private static int GetStatusCode(Exception exception) =>
            exception switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                ValidationException => StatusCodes.Status422UnprocessableEntity,
                TaskCanceledException => StatusCodes.Status400BadRequest,
                OperationCanceledException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

        private static string? GetTitle(Exception exception) =>
            exception switch
            {
                Infrastructure.Exceptions.ApplicationException applicationException => applicationException.Title,
                _ => "Server Error"
            };

        private static IReadOnlyDictionary<string, string[]>? GetErrors(Exception exception)
        {
            IReadOnlyDictionary<string, string[]>? errors = null;

            if (exception is ValidationException validationException)
            {
                errors = validationException.Errors;
            }

            return errors;
        }

    }
}