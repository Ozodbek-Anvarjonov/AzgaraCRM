using AzgaraCRM.WebUI.Models.Categories;
using FluentValidation;

namespace AzgaraCRM.WebUI.Validations.Categories;

public class CreateCategoryModelValidator : AbstractValidator<CreateCategoryModel>
{
    public CreateCategoryModelValidator()
    {
        RuleFor(entity => entity.Name)
            .NotNull().NotEmpty().WithMessage("Category name is required.")
            .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters.");
    }
}