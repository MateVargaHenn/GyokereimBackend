namespace Modules.Authentication.Application;

public interface IUserModule
{
    Task<UserInfoDto> GetUserInfoAsync(string userId);
}
