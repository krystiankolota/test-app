using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WeatherDisplayer.Domain.Exceptions;
namespace WeatherDisplayer.Web.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;
    private readonly ApiBehaviorOptions _behaviorOptions;

    public ExceptionHandlingMiddleware(
        ILogger<ExceptionHandlingMiddleware> logger,
        RequestDelegate next,
        IOptions<ApiBehaviorOptions> behaviorOptions)
    {
        _logger = logger;
        _next = next;
        _behaviorOptions = behaviorOptions.Value;
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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var problemDetails = exception switch
        {
            GeneralException generalException => new ProblemDetails
            {
                Status = generalException.StatusCode,
                Detail = generalException.Message
            },
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Detail = "An unexpected error occurred."
            }
        };

        _behaviorOptions.ClientErrorMapping.TryGetValue(problemDetails.Status!.Value, out var clientErrorData);
        problemDetails.Type = clientErrorData?.Link;
        problemDetails.Title = clientErrorData?.Title;
        problemDetails.Extensions["traceId"] = Activity.Current?.Id ?? context.TraceIdentifier;

        context.Response.StatusCode = problemDetails.Status.Value;
        context.Response.ContentType = "application/problem+json";

        switch (context.Response.StatusCode)
        {
            case >= 500:
                _logger.LogError(exception, "A server error occurred: {Message}", exception.Message);
                break;
            case >= 400:
                _logger.LogWarning("A client error occurred: {Message}", exception.Message);
                break;
        }

        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}
