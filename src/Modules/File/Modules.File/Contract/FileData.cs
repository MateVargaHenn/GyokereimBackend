namespace Modules.File.Contract;

public sealed record FileData(
    string FileName,
    string ContentType,
    Stream Content);
