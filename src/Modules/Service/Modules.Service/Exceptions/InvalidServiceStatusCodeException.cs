namespace Modules.Service.Exceptions;

public class InvalidServiceStatusCodeException(string message = "Érvénytelen szolgáltatás állapot kód") : Exception(message)
{
}
