using System.Text.Json;
using Impactt.API.Exceptions;

namespace Impactt.API.Middlewares;

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
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        int statusCode;
        if (exception is ApiException apiException)
        {
            statusCode = apiException.StatusCode;
        }
        else
        {
            statusCode = StatusCodes.Status500InternalServerError;
        }

        context.Response.StatusCode = statusCode;

        var response = new { error = exception.Message };
        var json = JsonSerializer.Serialize(response);
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsync(json);
    }
}