namespace Modules.File.Contract;

public sealed record FileUpload(
    string FileName,
    string ContentType,
    byte[] Content);
