using FluentValidation;
using FluentValidation.Results;

namespace AzgaraCRM.WebUI.Extensions;

public static class ValidationExtensions
{
    public static async Task<ValidationResult> EnsureValidationAsync<TObject>(
        this IValidator<TObject> validator,
        TObject instance,
        CancellationToken cancellationToken = default)
    {
        var result = await validator.ValidateAsync(instance, cancellationToken);

        if (!result.IsValid)
            throw new Domain.Exceptions.ValidationException(result.Errors);

        return result;
    }
}