namespace Modules.File.Contract.DataTransferObject;

public record FileResultDto(Stream Content,
    string ContentType,
    string FileName);
