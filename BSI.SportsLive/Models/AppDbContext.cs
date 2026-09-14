using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BSI.SportsLive.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Match> Matches { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<TournamentTeam> TournamentTeams { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<TeamSquad> TeamSquads { get; set; }
        public DbSet<MatchPlayingXI> MatchPlayingXIs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Composite key for TournamentTeam
            builder.Entity<TournamentTeam>().HasKey(tt => new { tt.TournamentId, tt.TeamId });

            // TournamentTeam relationships
            builder.Entity<TournamentTeam>()
                .HasOne(tt => tt.Tournament)
                .WithMany(t => t.TournamentTeams)
                .HasForeignKey(tt => tt.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TournamentTeam>()
                .HasOne(tt => tt.Team)
                .WithMany()
                .HasForeignKey(tt => tt.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            // Schedule relationships
            builder.Entity<Schedule>()
                .HasOne(s => s.Tournament)
                .WithMany()
                .HasForeignKey(s => s.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Schedule>()
                .HasOne(s => s.TeamA)
                .WithMany()
                .HasForeignKey(s => s.TeamAId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Schedule>()
                .HasOne(s => s.TeamB)
                .WithMany()
                .HasForeignKey(s => s.TeamBId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure composite primary key and relationships for MatchPlayingXI
            builder.Entity<MatchPlayingXI>(entity =>
            {
                entity.HasKey(x => new { x.MatchId, x.TeamId, x.PlayerId });

                entity.HasOne(x => x.Match)
                    .WithMany() // change to WithMany(m => m.MatchPlayingXIs) if collection exists
                    .HasForeignKey(x => x.MatchId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Team)
                    .WithMany()
                    .HasForeignKey(x => x.TeamId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Player)
                    .WithMany()
                    .HasForeignKey(x => x.PlayerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure composite primary key and relationships for TeamSquad
            builder.Entity<TeamSquad>(entity =>
            {
                entity.HasKey(x => new { x.TournamentId, x.TeamId, x.PlayerId });

                entity.HasOne(x => x.Tournament)
                    .WithMany() // change to WithMany(t => t.TeamSquads) if collection exists
                    .HasForeignKey(x => x.TournamentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Team)
                    .WithMany()
                    .HasForeignKey(x => x.TeamId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Player)
                    .WithMany()
                    .HasForeignKey(x => x.PlayerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
