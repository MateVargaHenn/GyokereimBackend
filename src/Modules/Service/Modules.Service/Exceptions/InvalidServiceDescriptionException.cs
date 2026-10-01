namespace Modules.Service.Exceptions;

public class InvalidServiceDescriptionException(string message = "Érvénytelen szolgáltatás leírás") : Exception(message)
{
}
