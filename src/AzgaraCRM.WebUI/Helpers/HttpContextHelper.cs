using System.Security.Claims;

namespace AzgaraCRM.WebUI.Helpers;

public static class HttpContextHelper
{
    public static IHttpContextAccessor Accessor { get; set; } = default!;

    public static HttpContext HttpContext => Accessor.HttpContext!;

    public static IHeaderDictionary ResponseHeaders => HttpContext.Response.Headers;

    public static long SystemId { get; set; }

    public static long? UserId
    {
        get
        {
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (long.TryParse(userIdClaim, out var userId))
                return userId;

            return null;
        }
    }
}