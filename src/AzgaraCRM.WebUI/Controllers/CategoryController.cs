using AutoMapper;
using AzgaraCRM.WebApi.Validations.Categories;
using AzgaraCRM.WebUI.Domain.Entities;
using AzgaraCRM.WebUI.Domain.Enums;
using AzgaraCRM.WebUI.Domain.Models;
using AzgaraCRM.WebUI.Extensions;
using AzgaraCRM.WebUI.Middlewares;
using AzgaraCRM.WebUI.Models.Categories;
using AzgaraCRM.WebUI.Models.Users;
using AzgaraCRM.WebUI.Services.Interfaces;
using AzgaraCRM.WebUI.Validations.Categories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AzgaraCRM.WebUI.Controllers;

[CustomAuthorize(nameof(UserRole.Admin), nameof(UserRole.Owner))]
public class CategoryController(
    IMapper mapper,
    IValidator<CreateCategoryModel> createCategoryValidator,
    IValidator<UpdateCategoryModel> updateCategoryValidator,
    ICategoryService categoryService) : BaseController
{
    [HttpGet]
    public async ValueTask<IActionResult> GetAll(
        [FromQuery] PaginationParameters @params,
        [FromQuery] SortingParameters sort,
        [FromQuery] string? search = null)
    {
        var categories = await categoryService.GetAllAsync(@params, sort, search, HttpContext.RequestAborted);
        var result = mapper.Map<IEnumerable<CategoryModelView>>(categories);

        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async ValueTask<IActionResult> Get([FromRoute] long id)
    {
        var category = await categoryService.GetByIdAsync(id, HttpContext.RequestAborted);
        var result = mapper.Map<CategoryModelView>(category);

        return Ok(result);
    }

    [CustomAuthorize(nameof(UserRole.Owner))]
    [HttpPost]
    public async ValueTask<IActionResult> Post([FromBody] CreateCategoryModel model)
    {
        _ = await createCategoryValidator.EnsureValidationAsync(model, HttpContext.RequestAborted);
        var category = await categoryService.CreateAsync(mapper.Map<Category>(model), HttpContext.RequestAborted);

        return Ok(mapper.Map<CategoryModelView>(category));
    }

    [HttpPut("{id:long}")]
    public async ValueTask<IActionResult> Put([FromRoute] long id, [FromBody] UpdateCategoryModel model)
    {
        _ = await updateCategoryValidator.EnsureValidationAsync(model, HttpContext.RequestAborted);
        var category = await categoryService.UpdateAsync(id, mapper.Map<Category>(model), HttpContext.RequestAborted);

        return Ok(mapper.Map<CategoryModelView>(category));
    }

    [CustomAuthorize(nameof(UserRole.Owner))]
    [HttpDelete("{id:long}")]
    public async ValueTask<IActionResult> Delete([FromRoute] long id)
    {
        await categoryService.DeleteByIdAsync(id, HttpContext.RequestAborted);

        return NoContent();
    }
}