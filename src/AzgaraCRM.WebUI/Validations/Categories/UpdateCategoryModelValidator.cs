using AzgaraCRM.WebUI.Models.Categories;
using AzgaraCRM.WebUI.Models.Users;
using FluentValidation;

namespace AzgaraCRM.WebApi.Validations.Categories;

public class UpdateCategoryModelValidator : AbstractValidator<UpdateCategoryModel>
{
    public UpdateCategoryModelValidator()
    {
        RuleFor(entity => entity.Name)
            .NotNull().NotEmpty().WithMessage("Category name is required.")
            .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters.");
        
        RuleFor(entity => entity.Id)
            .GreaterThan(0)
            .WithMessage("ID cannot be 0");
    }
}