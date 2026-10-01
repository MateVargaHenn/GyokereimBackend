namespace Modules.Blog.Exceptions;

public class PostTitleTooLongException(string message = "Blog címe túl hosszú!") : Exception(message);
