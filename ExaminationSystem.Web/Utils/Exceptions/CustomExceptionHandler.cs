using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Web.Utils.Exceptions;

public class CustomExceptionHandler(ILogger<CustomException> logger, IWebHostEnvironment environment) : IExceptionHandler
{

    /// <summary>
    /// 异常拦截器 return true结束 false 到下一个ExceptionHandler 找不到就默认UseExceptionHandler处理
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="exception"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, System.Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not CustomException customException) return false;
        logger.LogError(
            exception, "Exceptions occurred: {Message} {StackTrace} {Source}", exception.Message, exception.StackTrace, exception.Source);

        var problemDetails = new ProblemDetails
        {
            Status = customException.Code,
            Title = "This is Custom Exception"
        };
        if (environment.IsDevelopment())
        {
            problemDetails.Detail = $"Exceptions occurred: {customException.Message} {customException.StackTrace} {customException.Source}";
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;

    }
}