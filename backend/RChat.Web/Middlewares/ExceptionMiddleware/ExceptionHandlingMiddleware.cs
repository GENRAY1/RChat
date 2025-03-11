using System.Diagnostics;
using System.Net;
using System.Text.Json;
using RChat.Application.Exceptions;

namespace RChat.Web.Middlewares.ExceptionMiddleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        ExceptionResponse response = exception switch
        {
            ApiException apiException => new ExceptionResponse(apiException.StatusCode, apiException.Message),     
            _ => new ExceptionResponse((int)HttpStatusCode.InternalServerError, "Internal Server Error. Please try again later")
        };

        _logger.LogError(exception, exception.ToString());
        await WriteExceptionAsync(context, response);
    }

    private async Task WriteExceptionAsync(HttpContext context, ExceptionResponse response)
    {
        context.Response.StatusCode = response.StatusCode;
        context.Response.ContentType = "application/json";
        
        var jsonResponse = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(jsonResponse);
    }
}