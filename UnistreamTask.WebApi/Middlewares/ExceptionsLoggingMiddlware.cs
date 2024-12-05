using UnistreamTask.Application.Exceptions;

namespace UnistreamTask.WebApi.Middlewares;

public class ExceptionsLoggingMiddlware
{
    private readonly ILogger<ExceptionsLoggingMiddlware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionsLoggingMiddlware(RequestDelegate next, ILogger<ExceptionsLoggingMiddlware> logger)
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
        catch (NotExistedEntityException exception)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await WriteAndLogException(exception, context);
        }
        catch (DuplicatedEntityException exception)
        {
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await WriteAndLogException(exception, context);
        }
        catch (StorageOverfullException exception)
        {
            context.Response.StatusCode = StatusCodes.Status507InsufficientStorage;
            await WriteAndLogException(exception, context);
        }
    }

    private async Task WriteAndLogException(Exception exception, HttpContext context)
    {
        _logger.LogError(exception, exception.Message);
        await context.Response.WriteAsJsonAsync(exception.Message);
    }
}