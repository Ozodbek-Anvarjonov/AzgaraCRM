namespace AzgaraCRM.WebUI.Domain.Exceptions;

public class ArgumentIsNotValidException : Exception
{
    public ArgumentIsNotValidException()
    {
    }

    public ArgumentIsNotValidException(string message) : base(message)
    {
    }
}