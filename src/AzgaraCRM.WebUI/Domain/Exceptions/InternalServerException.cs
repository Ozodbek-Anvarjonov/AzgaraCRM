namespace AzgaraCRM.WebUI.Domain.Exceptions;

public class InternalServerException : Exception
{
    public InternalServerException() { }

    public InternalServerException(string message) : base(message) { }

    public InternalServerException(string message, Exception exception) { }

    public int StatusCode => 500;
}