namespace Modules.Blog.Exceptions;

public class PostTitleRequiredException(string message = "Blog címe nem lehet üres!") : Exception(message);
