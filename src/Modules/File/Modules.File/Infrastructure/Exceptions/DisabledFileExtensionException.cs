namespace Modules.File.Infrastructure.Exceptions;

public class DisabledFileExtensionException(string message = "A fájl kiterjesztés '{extension}' nem engedélyezett.") : Exception(message)
{
}
