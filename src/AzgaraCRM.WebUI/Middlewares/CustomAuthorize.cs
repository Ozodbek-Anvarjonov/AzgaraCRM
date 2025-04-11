using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AzgaraCRM.WebUI.Middlewares;

public class CustomAuthorize : Attribute, IAuthorizationFilter
{
    private readonly string[] _roles;

    public CustomAuthorize(params string[] roles) =>
        _roles = roles ?? Array.Empty<string>();

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var actionDescriptor = context.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;

        var allowAnonymous = actionDescriptor?.MethodInfo.GetCustomAttributes(inherit: true)
            .OfType<AllowAnonymousAttribute>().Any() ?? false;
        if (allowAnonymous) return;

        var user = context.HttpContext.User;

        var problem = new ProblemDetails();

        if (user?.Identity?.IsAuthenticated is not true)
        {
            problem.Status = StatusCodes.Status401Unauthorized;
            problem.Detail = "Authentication is required to access this resource.";

            context.Result = new ObjectResult(problem)
            {
                StatusCode = problem.Status,
            };

            return;
        }

        if (_roles.Length > 0 && !_roles.Any(user.IsInRole))
        {
            problem.Status = StatusCodes.Status403Forbidden;
            problem.Detail = "You do not have permission for this method.";

            context.Result = new ObjectResult(problem)
            {
                StatusCode = problem.Status,
            };

            return;
        }
    }
}