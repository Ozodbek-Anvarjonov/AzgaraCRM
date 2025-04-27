using AzgaraCRM.WebUI.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AzgaraCRM.WebUI.ExceptionHandlers;

public class CustomExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not CustomException)
            return false;

        var customException = (CustomException)exception;

        httpContext.Response.StatusCode = customException.StatusCode;
        httpContext.Response.ContentType = "application/json";

        var problem = new ProblemDetails
        {
            Status = customException.StatusCode,
            Detail = exception.Message,
        };

        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}