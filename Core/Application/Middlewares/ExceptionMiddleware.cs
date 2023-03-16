using Application.Exceptions;
using Application.Exceptions.ProblemDetails;
using Application.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Middlewares
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
            catch (Exception exception)
            {
                await HandleExceptionAsync(exception, context);
            }
        }
        Task HandleExceptionAsync(Exception exception, HttpContext context)
        {
            context.Response.ContentType = "application/json";

            if (exception.GetType() == typeof(ValidatorException)) return ValidatorExceptionHandle(context, exception);

            return InternalExceptionHandle(context, exception);
        }

        Task ValidatorExceptionHandle(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
            var errors = ((ValidatorException)exception).Errors;

            LogDetail logDetail = new()
            {
                RequestName = context.Request.Path,
                Exceptions = string.Join(", ", errors),
                MethodName = context.Request.Method
            };

            _logger.LogWarning(JsonSerializer.Serialize(logDetail));

            return context.Response.WriteAsync(new ValidatorProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validator Errors",
                Type = "https://example.com/probs/validation",
                Detail = "",
                Instance = "",
                Errors = errors
            }.ToString());
        }

        Task InternalExceptionHandle(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);

            LogDetail logDetail = new()
            {
                RequestName = context.Request.Path,
                Exceptions = exception.Message,
                MethodName = context.Request.Method
            };

            _logger.LogError(JsonSerializer.Serialize(logDetail));

            return context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Errors",
                Type = "https://example.com/probs/internal",
                Detail = exception.Message,
                Instance = "",
            }));
        }
    }
}
