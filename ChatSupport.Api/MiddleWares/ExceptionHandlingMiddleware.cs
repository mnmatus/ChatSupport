using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ChatSupport.Api.MiddleWares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (InvalidOperationException ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Invalid Request",
                    Error = ex.Message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (ValidationException ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Validation Error",
                    Error = ex.Message // Optional: Include for debugging
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Internal Server Error",
                    Error = ex.Message // Optional: Include for debugging
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
