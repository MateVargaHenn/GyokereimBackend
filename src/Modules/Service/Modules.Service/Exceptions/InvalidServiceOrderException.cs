namespace Modules.Service.Exceptions;

public class InvalidServiceOrderException(string message = "Érvénytelen szolgáltatás sorrend") : Exception(message)
{
}
