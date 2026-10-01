namespace Modules.Service.Exceptions;

public class InvalidServicePriceException(string message = "Érvénytelen szolgáltatás ár") : Exception(message)
{
}
