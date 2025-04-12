using AutoMapper;
using AzgaraCRM.WebUI.Domain.Entities;
using AzgaraCRM.WebUI.Domain.Models;
using AzgaraCRM.WebUI.Extensions;
using AzgaraCRM.WebUI.Models.Users;
using AzgaraCRM.WebUI.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AzgaraCRM.WebUI.Controllers;

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
        var result = mapper.Map<IEnumerable<UserViewModel>>(users);

        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async ValueTask<IActionResult> Get([FromRoute] long id)
    {
        var user = await userService.GetByIdAsync(id, HttpContext.RequestAborted);
        var result = mapper.Map<UserViewModel>(user);

        return Ok(result);
    }

    [HttpPost("admin")]
    public async ValueTask<IActionResult> Post([FromBody] CreateUserModel model)
    {
        _ = await createUserValidator.EnsureValidationAsync(model, HttpContext.RequestAborted);
        var user = await userService.CreateAsync(mapper.Map<User>(model), HttpContext.RequestAborted);

        return Ok(mapper.Map<UserViewModel>(user));
    }

    [HttpPut("{id:long}")]
    public async ValueTask<IActionResult> Put([FromRoute] long id, [FromBody] UpdateUserModel model)
    {
        _ = await updateUserValidator.EnsureValidationAsync(model, HttpContext.RequestAborted);
        var user = await userService.UpdateAsync(id, mapper.Map<User>(model), HttpContext.RequestAborted);

        return Ok(mapper.Map<UserViewModel>(user));
    }

    [HttpDelete("{id:long}")]
    public async ValueTask<IActionResult> Delete([FromRoute] long id)
    {
        await userService.DeleteByIdAsync(id, HttpContext.RequestAborted);

        return NoContent();
    }
}