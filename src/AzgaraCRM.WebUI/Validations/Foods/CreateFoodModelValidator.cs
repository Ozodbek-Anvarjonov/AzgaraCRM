using AzgaraCRM.WebUI.Models.Foods;
using FluentValidation;

namespace AzgaraCRM.WebUI.Validations.Foods;

public class CreateFoodModelValidator : AbstractValidator<CreateFoodModel>
{
    public CreateFoodModelValidator()
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
    }
}