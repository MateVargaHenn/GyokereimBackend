namespace Modules.Blog.Exceptions;

public class InvalidAuthorIdException(string message = "Érvénytelen szerző.") : Exception(message);
