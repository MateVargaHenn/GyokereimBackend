namespace Modules.File.Contract.DataTransferObject;

public sealed record UpdateFileMetadataDto(
    string? Title,
    string? AltText);
