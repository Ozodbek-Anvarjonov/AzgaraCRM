using AzgaraCRM.WebUI.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzgaraCRM.WebUI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = nameof(ActiveUserRequirement))]
public class BaseController : ControllerBase
{
}