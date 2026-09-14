using BSI.SportsLive.Models;
using Microsoft.EntityFrameworkCore;

namespace BSI.SportsLive.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(AppDbContext context)
        {
            // Apply pending migrations (will create DB/schema if needed)
            await context.Database.MigrateAsync();

            // Seed Teams and Players if none exist
            if (!context.Teams.Any())
            {
                var team1 = new Team
                {
                    Name = "Lahore Lions",
                    ShortName = "LL",
                    SportType = "Cricket"
                };

                var team2 = new Team
                {
                    Name = "Karachi Kings",
                    ShortName = "KK",
                    SportType = "Cricket"
                };

                context.Teams.AddRange(team1, team2);
                await context.SaveChangesAsync();

                var p1 = new Player { FullName = "Babar Azam", BroadcastName = "Babar A.", ShirtNumber = "56", TeamId = team1.Id };
                var p2 = new Player { FullName = "Shadab Khan", BroadcastName = "Shadab K.", ShirtNumber = "7", TeamId = team1.Id };
                var p3 = new Player { FullName = "Mohammad Amir", BroadcastName = "M. Amir", ShirtNumber = "90", TeamId = team2.Id };
                var p4 = new Player { FullName = "Shoaib Malik", BroadcastName = "S. Malik", ShirtNumber = "10", TeamId = team2.Id };

                context.Players.AddRange(p1, p2, p3, p4);
                await context.SaveChangesAsync();
            }

            // Seed one sample match if none exist
            if (!context.Matches.Any())
            {
                var match = new Match
                {
                    SportType = "Cricket",
                    TournamentName = "Demo Cup",
                    TeamA = context.Teams.OrderBy(t => t.Id).Select(t => t.Name).FirstOrDefault() ?? "Team A",
                    TeamB = context.Teams.OrderByDescending(t => t.Id).Select(t => t.Name).FirstOrDefault() ?? "Team B",
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
