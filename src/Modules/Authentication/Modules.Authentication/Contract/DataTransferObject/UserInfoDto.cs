namespace Modules.Authentication.Contract.DataTransferObject;

public record UserInfoDto(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string Username,
    byte[]? ProfilePicture
);
