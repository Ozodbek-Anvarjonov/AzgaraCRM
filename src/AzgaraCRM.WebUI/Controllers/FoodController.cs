using AutoMapper;
using AzgaraCRM.WebUI.Domain.Entities;
using AzgaraCRM.WebUI.Domain.Enums;
using AzgaraCRM.WebUI.Domain.Models;
using AzgaraCRM.WebUI.Extensions;
using AzgaraCRM.WebUI.Middlewares;
using AzgaraCRM.WebUI.Models.Foods;
using AzgaraCRM.WebUI.Models.Users;
using AzgaraCRM.WebUI.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AzgaraCRM.WebUI.Controllers;

[CustomAuthorize(nameof(UserRole.Admin), nameof(UserRole.Owner))]
public class FoodController(
    IMapper mapper,
    IValidator<CreateFoodModel> createFoodValidator,
    IValidator<UpdateFoodModel> updateFoodValidator,
    IFoodService foodService) : BaseController
{
    [HttpGet]
    [AllowAnonymous]
    public async ValueTask<IActionResult> GetAll(
        [FromQuery] PaginationParameters @params,
        [FromQuery] SortingParameters sort,
        [FromQuery] string? search = null)
    {
        var foods = await foodService.GetAllAsync(@params, sort, search, HttpContext.RequestAborted);
        var result = mapper.Map<IEnumerable<FoodModelView>>(foods);

        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [AllowAnonymous]
    public async ValueTask<IActionResult> Get([FromRoute] long id)
    {
        var food = await foodService.GetByIdAsync(id, HttpContext.RequestAborted);
        var result = mapper.Map<FoodModelView>(food);

        return Ok(result);
    }

    [CustomAuthorize(nameof(UserRole.Owner))]
    [HttpPost]
    public async ValueTask<IActionResult> Post(CreateFoodModel model)
    {
        _ = await createFoodValidator.EnsureValidationAsync(model, HttpContext.RequestAborted);
        var food = await foodService.CreateAsync(mapper.Map<Food>(model), model.File, HttpContext.RequestAborted);

        return Ok(mapper.Map<FoodModelView>(food));
    }

    [HttpPut("{id:long}")]
    public async ValueTask<IActionResult> Put([FromRoute] long id, UpdateFoodModel model)
    {
        _ = await updateFoodValidator.EnsureValidationAsync(model, HttpContext.RequestAborted);
        var food = await foodService.UpdateAsync(id, mapper.Map<Food>(model), model.File, HttpContext.RequestAborted);

        return Ok(mapper.Map<FoodModelView>(food));
    }

    [CustomAuthorize(nameof(UserRole.Owner))]
    [HttpDelete("{id:long}")]
    public async ValueTask<IActionResult> Delete([FromRoute] long id)
    {
        await foodService.DeleteByIdAsync(id, HttpContext.RequestAborted);

        return NoContent();
    }
}