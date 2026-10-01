namespace Modules.Authentication.Domain;

internal sealed class User : IdentityUser
{
    public string FirstName { get; set; } = default!; 
    public string LastName { get; set; } = default!;
    public int ProfileImageId { get; set; } = default!;
}
