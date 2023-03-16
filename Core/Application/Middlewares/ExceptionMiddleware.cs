using Application.Exceptions;
using Application.Exceptions.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Middlewares
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
            catch (Exception exception)
            {
                await HandleExceptionAsync(exception,context);
            }
        }
        Task HandleExceptionAsync(Exception exception, HttpContext context)
        {
            context.Response.ContentType = "application/json";

            if(exception.GetType() == typeof(ValidatorException)) return ValidatorExceptionHandle(context,exception);

            return InternalExceptionHandle(context, exception);
        }

        Task ValidatorExceptionHandle(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
            var errors = ((ValidatorException)exception).Errors;

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

        Task InternalExceptionHandle(HttpContext context,Exception exception)
        {
            context.Response.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);

            return context.Response.WriteAsync(new ProblemDetails 
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Errors",
                Type = "https://example.com/probs/internal",
                Detail = exception.Message,
                Instance = "",
            }.ToString());
        }
    }
}
