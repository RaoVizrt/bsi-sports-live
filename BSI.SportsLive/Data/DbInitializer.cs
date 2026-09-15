using BSI.SportsLive.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BSI.SportsLive.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(AppDbContext context, IServiceProvider serviceProvider)
        {
            // Apply pending migrations
            await context.Database.MigrateAsync();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // 1. Seed Roles
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 2. Seed Default Admin User
            var adminEmail = "admin@bsisports.com";
            var adminUser = await userManager.FindByNameAsync("admin");

            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(newAdmin, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
            else
            {
                // Ensure password and active state are correct on startup
                adminUser.IsActive = true;
                await userManager.UpdateAsync(adminUser);

                var token = await userManager.GeneratePasswordResetTokenAsync(adminUser);
                await userManager.ResetPasswordAsync(adminUser, token, "Admin@123");
            }

            // 3. Seed Teams and Global Players if none exist
            if (!context.Teams.Any())
            {
                var team1 = new Team { Name = "Lahore Lions", ShortName = "LL", SportType = "Cricket" };
                var team2 = new Team { Name = "Karachi Kings", ShortName = "KK", SportType = "Cricket" };

                context.Teams.AddRange(team1, team2);
                await context.SaveChangesAsync();

                var p1 = new Player { FullName = "Babar Azam", BroadcastName = "Babar A.", ShirtNumber = "56" };
                var p2 = new Player { FullName = "Shadab Khan", BroadcastName = "Shadab K.", ShirtNumber = "7" };
                var p3 = new Player { FullName = "Mohammad Amir", BroadcastName = "M. Amir", ShirtNumber = "90" };
                var p4 = new Player { FullName = "Shoaib Malik", BroadcastName = "S. Malik", ShirtNumber = "10" };

                context.Players.AddRange(p1, p2, p3, p4);
                await context.SaveChangesAsync();

                var teamPlayers = new List<TeamPlayer>
                {
                    new TeamPlayer { TeamId = team1.Id, PlayerId = p1.Id },
                    new TeamPlayer { TeamId = team1.Id, PlayerId = p2.Id },
                    new TeamPlayer { TeamId = team2.Id, PlayerId = p3.Id },
                    new TeamPlayer { TeamId = team2.Id, PlayerId = p4.Id }
                };

                context.TeamPlayers.AddRange(teamPlayers);
                await context.SaveChangesAsync();
            }

            // 4. Seed sample match if none exist
            if (!context.Matches.Any())
            {
                var teamA = await context.Teams.OrderBy(t => t.Id).FirstOrDefaultAsync();
                var teamB = await context.Teams.OrderByDescending(t => t.Id).FirstOrDefaultAsync();

                if (teamA != null && teamB != null)
                {
                    var match = new Match
                    {
                        SportType = "Cricket",
                        TournamentId = null, // Demo/friendly match — kisi tournament se linked nahi
                        TeamAId = teamA.Id,
                        TeamBId = teamB.Id,
                        Status = "Scheduled",
                        LiveScoreSummary = string.Empty,
                        MatchDate = DateTime.UtcNow.AddDays(1)
                    };
                    context.Matches.Add(match);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}