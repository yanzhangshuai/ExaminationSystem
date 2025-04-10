using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Web.Utils.Exceptions;

public class SystemExceptionHandle(ILogger<CustomException> logger, IWebHostEnvironment environment) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, System.Exception exception, CancellationToken cancellationToken)
    {
        if (exception is CustomException) return false;
        logger.LogError(
            exception, "Exceptions occurred: {Message} {StackTrace} {Source}", exception.Message, exception.StackTrace, exception.Source);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "这是系统异常:An error occurred while processing your request",
        };
        if (environment.IsDevelopment())
        {
            problemDetails.Detail = $"Exceptions occurred: {exception.Message} {exception.StackTrace} {exception.Source}";
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;

    }

}