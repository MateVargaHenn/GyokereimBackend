namespace Modules.Authentication.Contract.DataTransferObject;

public sealed record LoginRequestDto(
    string UserName,
    string Password);
