using Microsoft.Extensions.Options;
using Modules.Authentication.Options;

namespace Modules.Authentication.Infrastructure.Persistence.Seed;

internal class AuthenticationSeeder(
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager,
    IOptions<AppOptions> options)
{
    private readonly AppOptions _options = options.Value;
    internal async Task SeedAsync()
    {
        if (!await roleManager.RoleExistsAsync("SuperAdmin"))
        {
            IdentityResult roleResult = await roleManager.CreateAsync(
                new IdentityRole("SuperAdmin"));

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(
                        Environment.NewLine,
                        roleResult.Errors.Select(x => x.Description)));
            }
        }

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            IdentityResult roleResult = await roleManager.CreateAsync(
                new IdentityRole("Admin"));

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(
                        Environment.NewLine,
                        roleResult.Errors.Select(x => x.Description)));
            }
        }

        AppUserOptions superAdmin = _options.SuperAdmin;

        User? superAdminUser =
            await userManager.FindByNameAsync(superAdmin.User);

        if (superAdminUser is null)
        {
            superAdminUser = new User
            {
                FirstName = "Super",
                LastName = "Admin",
                ProfileImageId = -1,
                UserName = superAdmin.User,
                Email = superAdmin.Email,
                EmailConfirmed = true
            };

            IdentityResult result =
                await userManager.CreateAsync(
                    superAdminUser,
                    superAdmin.Password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(
                        Environment.NewLine,
                        result.Errors.Select(x => x.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(
                superAdminUser,
                "SuperAdmin"))
        {
            IdentityResult result =
                await userManager.AddToRoleAsync(
                    superAdminUser,
                    "SuperAdmin");

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(
                        Environment.NewLine,
                        result.Errors.Select(x => x.Description)));
            }
        }

        AppUserOptions admin = _options.Admin;

        User? adminUser =
            await userManager.FindByNameAsync(admin.User);

        if (adminUser is null)
        {
            adminUser = new User
            {
                FirstName = "Bánfi",
                LastName = "Zoltán Csaba",
                ProfileImageId = -1,
                UserName = admin.User,
                Email = admin.Email,
                EmailConfirmed = true
            };

            IdentityResult result =
                await userManager.CreateAsync(
                    adminUser,
                    admin.Password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(
                        Environment.NewLine,
                        result.Errors.Select(x => x.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(
                adminUser,
                "Admin"))
        {
            IdentityResult result =
                await userManager.AddToRoleAsync(
                    adminUser,
                    "Admin");

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(
                        Environment.NewLine,
                        result.Errors.Select(x => x.Description)));
            }
        }
    }
}
