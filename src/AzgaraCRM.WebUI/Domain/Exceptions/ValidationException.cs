using FluentValidation.Results;

namespace AzgaraCRM.WebUI.Domain.Exceptions;

public class ValidationException : Exception
{
    public IEnumerable<ValidationFailure> Errors { get; set; }

    public ValidationException(IEnumerable<ValidationFailure> errors)
        : base(string.Join(' ', errors.Select(error => error.ErrorMessage)))
    {
        Errors = errors;
    }
}