using AzgaraCRM.WebUI.Domain.Enums;
using AzgaraCRM.WebUI.Middlewares;
using AzgaraCRM.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AzgaraCRM.WebUI.Controllers;

[CustomAuthorize(nameof(UserRole.Admin), nameof(UserRole.Owner))]
public class StatisticsController(IStatisticsService statisticsService) : BaseController
{
    [AllowAnonymous]
    [HttpGet]
    public async ValueTask<IActionResult> Get()
    {
        var result = await statisticsService.GetAsync(HttpContext.RequestAborted);

        return Ok(result);
    }
}