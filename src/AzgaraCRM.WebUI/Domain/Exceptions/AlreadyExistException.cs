namespace AzgaraCRM.WebUI.Domain.Exceptions;

public class AlreadyExistException : Exception
{
    public AlreadyExistException()
    {
    }

    public AlreadyExistException(string entityName, object key)
        : base($"{entityName} with '{key}' already exists.")
    {
    }
}