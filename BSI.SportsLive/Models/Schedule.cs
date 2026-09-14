using System.ComponentModel.DataAnnotations;

namespace BSI.SportsLive.Models
{
    public class Schedule
    {
        public int Id { get; set; }

        public int TournamentId { get; set; }
        public Tournament? Tournament { get; set; }

        public int? MatchId { get; set; } // optional link to created Match

        public int MatchNumber { get; set; }

        public int TeamAId { get; set; }
        public int TeamBId { get; set; }

        // Navigation properties for teams
        public Team? TeamA { get; set; }
        public Team? TeamB { get; set; }

        public string Venue { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public DateTime? StartTime { get; set; }

        public string Status { get; set; } = "Scheduled"; // Scheduled, Live, Completed, Postponed
    }
}
