namespace Modules.File.Contract.DataTransferObject;

public sealed record AdminFileItemDto(
    int Id,
    string Key,
    string FileName,
    string ContentType,
    long Size,
    string? Title,
    string? AltText,
    bool IsPublic,
    DateTime LastModified);
