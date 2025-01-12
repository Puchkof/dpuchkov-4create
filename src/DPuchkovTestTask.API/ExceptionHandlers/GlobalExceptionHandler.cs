using System.Net;
using DPuchkovTestTask.Application.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace DPuchkovTestTask.API.ExceptionHandlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unexpected error occurred");

        var (statusCode, response) = exception switch
        {
            ValidationException validationEx => HandleValidationException(validationEx),
            NotFoundException notFoundEx => HandleNotFoundException(notFoundEx),
            _ => HandleUnknownException(exception)
        };

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }

    private static (int StatusCode, object Response) HandleValidationException(ValidationException exception)
    {
        var errors = exception.Errors.Select(e => new
        {
            Field = e.PropertyName,
            Error = e.ErrorMessage
        });

        return ((int)HttpStatusCode.BadRequest, new
        {
            Status = "Error",
            Message = "Validation failed",
            Details = errors
        });
    }

    private static (int StatusCode, object Response) HandleNotFoundException(NotFoundException exception)
    {
        return ((int)HttpStatusCode.NotFound, new
        {
            Status = "Error",
            exception.Message,
            Details = (object?)null
        });
    }

    private static (int StatusCode, object Response) HandleUnknownException(Exception exception)
    {
        return ((int)HttpStatusCode.InternalServerError, new
        {
            Status = "Error",
            Message = "An unexpected error occurred",
            Details = (object?)null
        });
    }
} 