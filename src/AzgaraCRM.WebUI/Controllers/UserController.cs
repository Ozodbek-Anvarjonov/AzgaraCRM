using AutoMapper;
using AzgaraCRM.WebUI.Domain.Entities;
using AzgaraCRM.WebUI.Domain.Enums;
using AzgaraCRM.WebUI.Domain.Models;
using AzgaraCRM.WebUI.Extensions;
using AzgaraCRM.WebUI.Middlewares;
using AzgaraCRM.WebUI.Models.Users;
using AzgaraCRM.WebUI.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AzgaraCRM.WebUI.Controllers;

[CustomAuthorize(nameof(UserRole.Admin), nameof(UserRole.Owner))]
public class UserController(
    IMapper mapper,
    IValidator<CreateUserModel> createUserValidator,
    IValidator<UpdateUserModel> updateUserValidator,
    IUserService userService) : BaseController
{
    [HttpGet]
    public async ValueTask<IActionResult> GetAll(
        [FromQuery] PaginationParameters @params,
        [FromQuery] SortingParameters sort,
        [FromQuery] string? search = null)
    {
        var users = await userService.GetAllAsync(@params, sort, search, HttpContext.RequestAborted);
        var result = mapper.Map<IEnumerable<UserModelView>>(users);

        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async ValueTask<IActionResult> Get([FromRoute] long id)
    {
        var user = await userService.GetByIdAsync(id, HttpContext.RequestAborted);
        var result = mapper.Map<UserModelView>(user);

        return Ok(result);
    }

    [CustomAuthorize(nameof(UserRole.Owner))]
    [HttpPost("admin")]
    public async ValueTask<IActionResult> Post([FromBody] CreateUserModel model)
    {
        _ = await createUserValidator.EnsureValidationAsync(model, HttpContext.RequestAborted);
        var user = await userService.CreateAsync(mapper.Map<User>(model), HttpContext.RequestAborted);

        return Ok(mapper.Map<UserModelView>(user));
    }

    [HttpPut("{id:long}")]
    public async ValueTask<IActionResult> Put([FromRoute] long id, [FromBody] UpdateUserModel model)
    {
        _ = await updateUserValidator.EnsureValidationAsync(model, HttpContext.RequestAborted);
        var user = await userService.UpdateAsync(id, mapper.Map<User>(model), HttpContext.RequestAborted);

        return Ok(mapper.Map<UserModelView>(user));
    }

    [CustomAuthorize(nameof(UserRole.Owner))]
    [HttpPatch("{id:long}/enable")]
    public async ValueTask<IActionResult> Enable([FromRoute] long id)
    {
        _ = await userService.EnableAsync(id, HttpContext.RequestAborted);
        return Ok();
    }


    [CustomAuthorize(nameof(UserRole.Owner))]
    [HttpPatch("{id:long}/disable")]
    public async ValueTask<IActionResult> Disable([FromRoute] long id)
    {
        _ = await userService.DisableAsync(id, HttpContext.RequestAborted);
        return Ok();
    }

    [CustomAuthorize(nameof(UserRole.Owner))]
    [HttpDelete("{id:long}")]
    public async ValueTask<IActionResult> Delete([FromRoute] long id)
    {
        await userService.DeleteByIdAsync(id, HttpContext.RequestAborted);

        return NoContent();
    }
}