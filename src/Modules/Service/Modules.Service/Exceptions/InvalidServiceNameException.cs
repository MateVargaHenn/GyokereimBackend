namespace Modules.Service.Exceptions;

public class InvalidServiceNameException(string message = "Érvénytelen szolgáltatás név") : Exception(message)
{
}
