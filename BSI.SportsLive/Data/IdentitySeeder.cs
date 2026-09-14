using BSI.SportsLive.Models;
using Microsoft.AspNetCore.Identity;

namespace BSI.SportsLive.Data
{
    public static class IdentitySeeder
    {
        private static readonly string[] Roles = new[] { "SuperAdmin", "TournamentAdmin", "Scorer", "ReplayOperator", "Graphics" };

        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Create roles
            foreach (var role in Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Create default super admin
            var adminEmail = "admin@localhost";
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new ApplicationUser { UserName = "admin", Email = adminEmail, EmailConfirmed = true };
                var result = await userManager.CreateAsync(admin, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "SuperAdmin");
                }
            }
        }
    }
}
