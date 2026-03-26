using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace oop_s2_2_mvc_71757.Data;

public static class IdentitySeeder
{
    private const string AdminRole = "Admin";
    private const string InspectorRole = "Inspector";
    private const string ViewerRole = "Viewer";

    public static async Task SeedAsync(IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("IdentitySeeder");

        await EnsureRoleAsync(roleManager, AdminRole, logger);
        await EnsureRoleAsync(roleManager, InspectorRole, logger);
        await EnsureRoleAsync(roleManager, ViewerRole, logger);

        await EnsureUserAsync(userManager, logger, AdminRole, "admin@foodsafety.local", "Admin123!");
        await EnsureUserAsync(userManager, logger, InspectorRole, "inspector@foodsafety.local", "Inspector123!");
        await EnsureUserAsync(userManager, logger, ViewerRole, "viewer@foodsafety.local", "Viewer123!");
    }

    private static async Task EnsureRoleAsync(RoleManager<IdentityRole> roleManager, string role, ILogger logger)
    {
        if (await roleManager.RoleExistsAsync(role))
        {
            return;
        }

        var result = await roleManager.CreateAsync(new IdentityRole(role));
        if (!result.Succeeded)
        {
            logger.LogWarning("Failed to create role {Role}: {Errors}", role, result.Errors);
        }
    }

    private static async Task EnsureUserAsync(
        UserManager<IdentityUser> userManager,
        ILogger logger,
        string role,
        string email,
        string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
            {
                logger.LogWarning("Failed to create user {Email}: {Errors}", email, createResult.Errors);
                return;
            }
        }

        if (!await userManager.IsInRoleAsync(user, role))
        {
            var addToRoleResult = await userManager.AddToRoleAsync(user, role);
            if (!addToRoleResult.Succeeded)
            {
                logger.LogWarning("Failed to add user {Email} to role {Role}: {Errors}", email, role, addToRoleResult.Errors);
            }
        }
    }
}
