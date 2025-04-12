using AzgaraCRM.WebUI.Models.Users;
using FluentValidation;

namespace AzgaraCRM.WebUI.Validations.Users;

public class CreateUserModelValidator : AbstractValidator<CreateUserModel>
{
    public CreateUserModelValidator()
    {
        RuleFor(entity => entity.FirstName)
            .NotNull().NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");

        RuleFor(entity => entity.LastName)
            .NotNull().NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.");

        RuleFor(entity => entity.Email)
            .NotNull().NotEmpty().WithMessage("User name is required.")
            .MinimumLength(6).WithMessage("User name must be at least 6 characters long.")
            .MaximumLength(100).WithMessage("User name cannot exceed 100 characters.");

        RuleFor(entity => entity.Password)
            .NotNull().NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .MaximumLength(100).WithMessage("Password cannot exceed 100 characters.");
    }
}