using AzgaraCRM.WebUI.Domain.Enums;
using AzgaraCRM.WebUI.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AzgaraCRM.WebUI.Middlewares;

public class ActiveUserRequirementHandler(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork) : AuthorizationHandler<ActiveUserRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ActiveUserRequirement requirement)
    {
        if (context.User?.Identity?.IsAuthenticated is not true)
        {
            context.Succeed(requirement);
            return;
        }

        var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value;
        var check = long.TryParse(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId);

        if (string.IsNullOrEmpty(userRole) || userRole != UserRole.Admin.ToString() || !check)
        {
            context.Succeed(requirement);
            return;
        }

        var user = await unitOfWork.Users.SelectAsync(entity => entity.Id == userId && !entity.IsDeleted);

        if (user is null || user.IsDisabled)
        {
            var httpContext = httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                httpContext.Response.ContentType = "application/json";

                var problem = new ProblemDetails
                {
                    Status = StatusCodes.Status403Forbidden,
                    Detail = "User is disabled or not found."
                };

                await httpContext.Response.WriteAsJsonAsync(problem);
            }

            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }
}