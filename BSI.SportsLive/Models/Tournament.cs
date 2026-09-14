using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BSI.SportsLive.Models
{
    public class Tournament
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string ShortName { get; set; } = string.Empty;

        public string Season { get; set; } = string.Empty;

        public string LogoUrl { get; set; } = string.Empty;

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Sport { get; set; } = "Cricket";

        public string StageFormat { get; set; } = "Knockout";

        public int ParticipantsCount { get; set; } = 8;

        public string Format { get; set; } = "T20"; // T20, ODI, Test, etc.

        public int OversPerInnings { get; set; } = 20;

        public string Status { get; set; } = "Planned"; // Planned, Ongoing, Completed

        // Kis user ne tournament banaya hai uski tracking ke liye
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        // Navigation
        public ICollection<TournamentTeam> TournamentTeams { get; set; } = new List<TournamentTeam>();
    }
}