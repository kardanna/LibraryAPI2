using LibraryAPI2.Application.Exceptions;
using FluentValidation;

namespace LibraryAPI2.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionHandlingMiddleware(RequestDelegate next)
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
            await HadleExceptionAsync(context, ex);
        }
    }

    private static Task HadleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = ex switch
        {
            EntityDoesNotExistException => StatusCodes.Status404NotFound,
            ValidationException => StatusCodes.Status400BadRequest,
            BadRequestException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        return context.Response.WriteAsJsonAsync(new
        {
            status = context.Response.StatusCode,
            error = ex.Message
        });
    }
}