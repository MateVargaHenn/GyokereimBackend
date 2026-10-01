namespace Modules.Service.Exceptions;

public class InvalidServiceShortDescriptionException(string message = "Érvénytelen szolgáltatás rövid leírás") : Exception(message)
{
}
