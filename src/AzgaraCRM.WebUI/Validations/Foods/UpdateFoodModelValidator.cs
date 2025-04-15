using AzgaraCRM.WebUI.Models.Foods;
using FluentValidation;

namespace AzgaraCRM.WebApi.Validations.Foods;

public class UpdateFoodModelValidator : AbstractValidator<UpdateFoodModel>
{
    public UpdateFoodModelValidator()
    {
        RuleFor(entity => entity.Name)
            .NotNull().NotEmpty().WithMessage("Food name is required.")
            .MaximumLength(100).WithMessage("Food name cannot exceed 100 characters.");

        RuleFor(entity => entity.Left)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Lefted food cannot be lower than 0.");

        RuleFor(entity => entity.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Food price cannot be lower than 0.");

        RuleFor(entity => entity.Id)
            .GreaterThan(0)
            .WithMessage("ID cannot be 0");

        RuleFor(entity => entity.CategoryId)
            .GreaterThan(0)
            .WithMessage("ID cannot be 0");
    }
}