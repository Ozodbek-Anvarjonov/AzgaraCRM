using AutoMapper;
using AzgaraCRM.WebUI.Models.Login;
using AzgaraCRM.WebUI.Models.Users;
using AzgaraCRM.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AzgaraCRM.WebUI.Controllers;

public class AuthController(IAccountService accountService, IMapper mapper) : BaseController
{
    [HttpPost("sing-in")]
    public async ValueTask<IActionResult> SignIn([FromBody] SignInModel model)
    {
        var token = await accountService.SignInAsync(model.Email, model.Password);

        var result = new
        {
            User = mapper.Map<UserModelView>(token.User),
            Token = token.Token,
        };

        return Ok(result);
    }
}