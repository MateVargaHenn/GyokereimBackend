namespace Modules.Service.Exceptions;

public class InvalidServiceSlugException(string message = "Érvénytelen szolgáltatás útvonal") : Exception(message)
{
}
