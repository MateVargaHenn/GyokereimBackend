using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Authentication.Infrastructure.Persistence.Seed;
using Modules.Authentication.Options;

namespace Modules.Authentication;

public static class AuthenticationModule
{
    private static readonly Assembly _module = typeof(AuthenticationModule).Assembly;
    public static void AddAuthentication(this IHostApplicationBuilder builder)
    {
        builder
            .Services
            .AddHttpContextAccessor();

        builder
            .Services
            .AddValidatorsFromAssembly(_module);

        builder.Services
            .AddOptions<AppOptions>()
            .Bind(builder.Configuration.GetSection("APP"))
            .Validate(
                x => !string.IsNullOrWhiteSpace(x.SuperAdmin.User),
                "A SuperAdmin felhasználónév nincs beállítva.")
            .Validate(
                x => !string.IsNullOrWhiteSpace(x.SuperAdmin.Password),
                "A SuperAdmin jelszó nincs beállítva.")
            .Validate(
                x => !string.IsNullOrWhiteSpace(x.SuperAdmin.Email),
                "A SuperAdmin e-mail nincs beállítva.")
            .ValidateOnStart();

        string connectionString =
            builder.Configuration.GetConnectionString("CONNECTION_STRING")
            ?? throw new InvalidOperationException(
                "ConnectionStrings:CONNECTION_STRING is not configured.");

        builder.Services.AddDbContext<AuthenticationDbContext>(options =>
            options.UseNpgsql(connectionString));


        builder.Services
                .AddAuthentication(IdentityConstants.BearerScheme)
                .AddBearerToken(IdentityConstants.BearerScheme);

        builder.Services.AddAuthorization(options => options.AddPolicy(
                "AdminOnly",
                policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Admin", "SuperAdmin");
                }));

        builder.Services
                .AddIdentityCore<User>(options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireDigit = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AuthenticationDbContext>()
                .AddSignInManager();

        builder
            .Services
            .AddScoped<AuthenticationSeeder>();

        builder
            .Services
            .AddHostedService<AuthenticationInitializer>();
    }

    public static void MapAuthenticationEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        RouteGroupBuilder group = endpoints
            .MapGroup("/api/authentication")
            .WithTags("Authentication");

        group
            .MapPost("/login", Login)
            .AllowAnonymous();

        group
            .MapGet("/me", Me)
            .RequireAuthorization();
    }

    private static async Task<IResult> Login(
    LoginRequestDto request,
    UserManager<User> userManager,
    SignInManager<User> signInManager)
    {
        User? user = await userManager.FindByNameAsync(request.UserName);

        if (user is null)
        {
            return Results.Unauthorized();
        }

        bool validPassword =
            await userManager.CheckPasswordAsync(
                user,
                request.Password);

        if (!validPassword)
        {
            return Results.Unauthorized();
        }

        ClaimsPrincipal principal =
            await signInManager.CreateUserPrincipalAsync(user);

        return Results.SignIn(
            principal,
            authenticationScheme: IdentityConstants.BearerScheme);
    }

    private static IResult Me(ClaimsPrincipal user) => Results.Ok(new
    {
        Name = user.Identity?.Name!,
        Roles = user.Claims
                .Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value)
    });
}
