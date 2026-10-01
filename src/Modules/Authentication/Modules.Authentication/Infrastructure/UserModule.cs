namespace Modules.Authentication.Infrastructure;

internal class UserModule(AuthenticationDbContext context) : IUserModule
{
    private readonly AuthenticationDbContext _context = context;
    public async Task<UserInfoDto> GetUserInfoAsync(string userId)
    {
        UserInfoDto? user = await _context.Users.Where(x => x.Id == userId).Select(x => new UserInfoDto(
            x.Id,
            x.FirstName,
            x.LastName,
            x.Email,
            x.UserName,
            null
        )).FirstOrDefaultAsync() ?? throw new InvalidOperationException($"User with ID '{userId}' not found.");

        return user;
    }
}
